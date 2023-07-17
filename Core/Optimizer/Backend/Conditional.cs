using System.Net.Mime;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Optimizer.Backend;

public partial class ScratchIRBackendVisitor
{
    public override object VisitIfCommand(ScratchIRParser.IfCommandContext context)
    {
        return Visit(context.ifStatement());
    }

    public override object VisitElseIfStatement(ScratchIRParser.ElseIfStatementContext context)
    {
        if (context.ifStatement() != null)
            return Visit(context.ifStatement());
        if (context.command() != null)
            return VisitCommands(context.command());
        return null;
    }

    public override object VisitRepeatCommand(ScratchIRParser.RepeatCommandContext context)
    {
        var amount = Visit(context.expression());

        var block = Control.Repeat();
        block.SetInput("TIMES", ScratchInput.New(amount, block));
        
        var blocks = VisitCommands(context.command());
        if (blocks.Count != 0)
        {
            var first = blocks.First();
            first.SetParent(block, false);
            UpdateBlocks(first);
            block.SetInput("SUBSTACK", ScratchInput.New(first));
        }
        
        UpdateBlocks(block, amount);
        return block;
    }

    public override object VisitIfStatement(ScratchIRParser.IfStatementContext context)
    {
        var condition = Visit(context.expression()) as Block;

        var block = context.elseIfStatement() != null ? Control.IfElse() : Control.If();
        block.SetInput("CONDITION", ScratchInput.New(condition, block));

        var blocks = VisitCommands(context.command());
        if (blocks.Count != 0)
        {
            var first = blocks.First();
            first.SetParent(block, false);
            UpdateBlocks(first);
            block.SetInput("SUBSTACK", ScratchInput.New(first));
        }

        if (context.elseIfStatement() != null)
        {
            var elseStack = Visit(context.elseIfStatement());
            switch (elseStack)
            {
                case List<Block> elseBlocks when elseBlocks.Any():
                {
                    var first = elseBlocks.First();
                    first.SetParent(block, false);
                    UpdateBlocks(first);
                    block.SetInput("SUBSTACK2", ScratchInput.New(first));
                    break;
                }
                case Block b:
                    b.SetParent(block, false);
                    UpdateBlocks(b);
                    block.SetInput("SUBSTACK2", ScratchInput.New(b));
                    break;
            }
        }
        
        UpdateBlocks(block);
        return block;
    }

    public override object VisitWhileCommand(ScratchIRParser.WhileCommandContext context)
    {
        var condition = Visit(context.expression());
        var block = Control.While();
        block.SetInput("CONDITION", ScratchInput.New(condition, block));
        UpdateBlocks(condition);
        
        var blocks = VisitCommands(context.command());
        if (blocks.Count != 0)
        {
            var first = blocks.First();
            first.SetParent(block, false);
            UpdateBlocks(first);
            block.SetInput("SUBSTACK", ScratchInput.New(first));
        }
        
        UpdateBlocks(block);
        return block;
    }

    private List<Block> VisitCommands(IEnumerable<ScratchIRParser.CommandContext> commands)
    {
        Block previousParent = _currentParent, first = null;
        _currentParent = null;
        var blocks = HandleCommands(commands);
        _currentParent = previousParent;
        return blocks;
    }
}