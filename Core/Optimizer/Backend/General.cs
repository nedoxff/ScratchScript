using System.Globalization;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Frontend.Implementation;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Optimizer.Backend;

public partial class ScratchIRBackendVisitor: ScratchIRBaseVisitor<object>
{
    public Target Target = new();
    private Dictionary<string, Block> Blocks => Target.Blocks;
    private Block _currentParent;

    public ScratchIRBackendVisitor(string name)
    {
        Target.Name = name;
        RegisterVariable(ScratchScriptVisitor.StackName, ScratchType.List(ScratchType.Any));
        RegisterVariable("__CopyList", ScratchType.String);
        RegisterVariable("__TempValue", ScratchType.Any);
        RegisterVariable("__CleanupCounter", ScratchType.Number);
    }

    private void UpdateBlocks(params object[] blocks)
    {
        foreach (var block in blocks)
        {
            if (block is not Block b) continue;
            Blocks[b.Id] = b;
        }
    }

    public override object VisitConstant(ScratchIRParser.ConstantContext context)
    {
        if (context.Number() is { } n)
            return decimal.Parse(n.GetText(), CultureInfo.InvariantCulture);
        if (context.String() is { } s)
            return s.GetText()[1..^1];
        if (context.Color() is { } c)
            return new ScratchColor(c.GetText()[1..]);
        return null;
    }

    public override object VisitEventBlock(ScratchIRParser.EventBlockContext context)
    {
        var lastParent = _currentParent;
        var block = context.Event().GetText() switch
        {
            "start" => Event.WhenFlagClicked(),
            _ => throw new ArgumentOutOfRangeException()
        };

        _currentParent = block;
        UpdateBlocks(block);
        HandleCommands(context.command());
        _currentParent = lastParent;
        return block;
    }

    public override object VisitParenthesizedExpression(ScratchIRParser.ParenthesizedExpressionContext context)
    {
        return Visit(context.expression());
    }

    private List<Block> HandleCommands(IEnumerable<ScratchIRParser.CommandContext> commands)
    {
        var resultBlocks = new List<Block>();
        foreach (var command in commands)
        {
            var result = Visit(command);
            switch (result)
            {
                case null:
                    continue;
                case Block { Shadow: false } block:
                {
                    if (_currentParent != null) block.SetParent(_currentParent);
                    _currentParent = block;
                    UpdateBlocks(block);
                    resultBlocks.Add(block);
                    break;
                }
                case List<Block> blocks:
                {
                    var first = blocks.First();
                    if(_currentParent != null) first.SetParent(_currentParent);
                    UpdateBlocks(first);
                    _currentParent = blocks.Last();
                    resultBlocks.AddRange(blocks);
                    break;
                }
            }
        }

        return resultBlocks;
    }

    public override object VisitFlagTopLevelStatement(ScratchIRParser.FlagTopLevelStatementContext context)
    {
        switch (context.Identifier().GetText())
        {
            case "UNICODE":
            {
                var characters = File.ReadAllLines(Path.Join(Environment.CurrentDirectory, "std", "unicode.txt"));
                var symbols = File.ReadAllText(Path.Join(Environment.CurrentDirectory, "std", "symbols.txt"));
                Target.Lists.Add("__Unicode", new List<object>
                {
                    "__Unicode",
                    characters
                });
                Target.Variables.Add("__Symbols", new List<object>
                {
                    "__Symbols",
                    symbols
                });
                var emptyCostume = CostumeHelper.GetEmptyCostume();
                emptyCostume.Name = "";
                Target.Costumes.Add(emptyCostume);
                var uppercaseCostume = CostumeHelper.GetEmptyCostume();
                uppercaseCostume.Name = File.ReadAllText(Path.Join(Environment.CurrentDirectory, "std", "uppercase.txt"));
                Target.Costumes.Add(uppercaseCostume);
                break;
            }
        }

        return null;
    }
}