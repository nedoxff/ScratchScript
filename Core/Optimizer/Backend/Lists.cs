using ScratchScript.Core.Blocks;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Optimizer.Backend;

public partial class ScratchIRBackendVisitor
{
    public override object VisitPushCommand(ScratchIRParser.PushCommandContext context)
    {
        var name = context.Identifier().GetText();
        var item = Visit(context.expression());

        var block = Data.AddToList();
        block.SetField("LIST", ScratchField.New(name));
        block.SetInput("ITEM", ScratchInput.New(item, block));
        UpdateBlocks(block, item);

        return block;
    }

    public override object VisitPushAtCommand(ScratchIRParser.PushAtCommandContext context)
    {
        var name = context.Identifier().GetText();
        var at = Visit(context.expression(0));
        var item = Visit(context.expression(1));

        var block = Data.InsertIntoList();
        block.SetField("LIST", ScratchField.New(name));
        block.SetInput("INDEX", ScratchInput.New(at));
        block.SetInput("ITEM", ScratchInput.New(item, block));
        UpdateBlocks(block, at, item);
        
        return block;
    }

    public override object VisitPopCommand(ScratchIRParser.PopCommandContext context)
    {
        var name = context.Identifier().GetText();

        var lengthOfList = Data.LengthOfList();
        lengthOfList.SetField("LIST", ScratchField.New(name));

        var block = Data.DeleteFromList();
        block.SetField("LIST", ScratchField.New(name));
        block.SetInput("INDEX", ScratchInput.New(lengthOfList, block));

        UpdateBlocks(lengthOfList, block);
        return block;
    }

    public override object VisitPopAtCommand(ScratchIRParser.PopAtCommandContext context)
    {
        var name = context.Identifier().GetText();
        var at = Visit(context.expression());

        var block = Data.DeleteFromList();
        block.SetField("LIST", ScratchField.New(name));
        block.SetInput("INDEX", ScratchInput.New(at));

        return block;
    }

    public override object VisitPopAllCommand(ScratchIRParser.PopAllCommandContext context)
    {
        var name = context.Identifier().GetText();

        var block = Data.DeleteAllOfList();
        block.SetField("LIST", ScratchField.New(name));
        
        return block;
    }

    public override object VisitListAccessExpression(ScratchIRParser.ListAccessExpressionContext context)
    {
        var name = context.Identifier().GetText();
        var expression = Visit(context.expression());

        var block = Data.ItemOfList();
        block.SetField("LIST", ScratchField.New(name));
        block.SetInput("INDEX", ScratchInput.New(expression));

        return block;
    }
}