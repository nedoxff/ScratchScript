﻿using Antlr4.Runtime.Atn;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Frontend.Implementation;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Optimizer.Backend;

public partial class ScratchIRBackendVisitor
{
    private List<ScratchProcedure> _procedures = new();

    public override object VisitProcedureBlock(ScratchIRParser.ProcedureBlockContext context)
    {
        var name = context.Identifier().GetText();
        var arguments = new Dictionary<string, ScratchProcedure.ProcedureArgumentType>();
        foreach (var argument in context.procedureArgument())
        {
            arguments[argument.Identifier().GetText()] =
                argument.procedureArgumentTypeDeclaration().ProcedureType().GetText() == ":sn"
                    ? ScratchProcedure.ProcedureArgumentType.StringNumber
                    : ScratchProcedure.ProcedureArgumentType.Boolean;
        }

        var procedure = new ScratchProcedure(name, context.WarpIdentifier() != null, arguments);
        _procedures.Add(procedure);
        UpdateProcedure();

        var blocks = VisitCommands(context.command());
        if (blocks.Any())
        {
            var first = blocks.First();
            first.SetParent(procedure.Definition);
            UpdateBlocks(first);
        }

        procedure.Build();
        UpdateProcedure();
        return procedure.Definition;
    }

    public override object VisitCallCommand(ScratchIRParser.CallCommandContext context)
    {
        var name = context.Identifier().GetText();
        var procedure = _procedures.First(x => x.Name == name);
        var block = procedure.Call.Clone();

        if (procedure.StackIndexReporter != null)
        {
            var lengthOfStack = Data.LengthOfList();
            lengthOfStack.SetField("LIST", ScratchField.New(ScratchScriptVisitor.StackName));
            block.SetInput(procedure.StackIndexReporter.Id, ScratchInput.New(lengthOfStack, block));
            UpdateBlocks(lengthOfStack);
        }
        
        UpdateBlocks(block);

        return block;
    }

    public override object VisitRawCommand(ScratchIRParser.RawCommandContext context)
    {
        var opcode = context.Identifier().GetText();
        var block = new Block(opcode, "raw");
        SetRawBlockProperties(ref block, context.callProcedureArgument());
        return block;
    }

    public override object VisitRawShadowExpression(ScratchIRParser.RawShadowExpressionContext context)
    {
        var opcode = context.Identifier().GetText();
        var block = new Block(opcode, "raws", shadow: true);
        SetRawBlockProperties(ref block, context.callProcedureArgument());
        return block;
    }

    private void SetRawBlockProperties(ref Block block,
        IEnumerable<ScratchIRParser.CallProcedureArgumentContext> arguments)
    {
        foreach (var argument in arguments)
        {
            var name = argument.Identifier().GetText();
            var expression = Visit(argument.expression());
            if (argument.procedureArgumentType().GetText() == "i:")
                block.SetInput(name, ScratchInput.New(expression, block));
            else
                block.SetField(name, ScratchField.New(expression));
            UpdateBlocks(block, expression);
        }
    }

    private void UpdateProcedure()
    {
        var procedure = _procedures.Last();
        var toUpdate = new List<object> { procedure.Definition, procedure.Prototype };
        if(procedure.StackIndexReporter != null) toUpdate.Add(procedure.StackIndexReporter);
        UpdateBlocks(toUpdate.ToArray());
    }
}