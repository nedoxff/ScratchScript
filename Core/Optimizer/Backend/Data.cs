using ScratchScript.Core.Blocks;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;
using Serilog;

namespace ScratchScript.Core.Optimizer.Backend;

public partial class ScratchIRBackendVisitor
{
    private List<ScratchVariable> _variables = new();
    public override object VisitSetCommand(ScratchIRParser.SetCommandContext context)
    {
        var name = context.variableIdentifier().Identifier().GetText();
        var variable = _variables.First(x => x.Name == name);
        var variableBlock = Data.Variable(variable);
        var value = Visit(context.expression());
        
        Log.Verbose("[Set] {Name} = {Value}", name, value);
        
        var block = Data.SetVariableTo();
        block.SetField("VARIABLE", ScratchField.FromVariableBlock(variableBlock));
        block.SetInput("VALUE", ScratchInput.New(value, block));
        UpdateBlocks(variableBlock, block, value);
        return block;
    }

    public override object VisitLoadCommand(ScratchIRParser.LoadCommandContext context)
    {
        var name = context.Identifier().GetText();
        Log.Verbose("[Load] Creating a variable named {Name}", name);
        var id = $"{Target.Name}_{name}";
        var type = TypeHelper.ScratchTypeFromString(context.Type().GetText()[1..]);
        
        _variables.Add(new ScratchVariable
        {
            Id = id,
            Name = name,
            Type = type
        });
        Target.Variables[id] = new List<object> {name, ""};
        return null;
    }

    public override object VisitVariableExpression(ScratchIRParser.VariableExpressionContext context)
    {
        var name = context.variableIdentifier().Identifier().GetText();
        if (context.variableIdentifier().ArgumentReporterIdentifier() == null)
            return Data.Variable(_variables.First(x => x.Name == name));

        var reporter = _procedures.Last().Arguments[name].Clone();
        UpdateBlocks(reporter);
        return reporter;
    }
}