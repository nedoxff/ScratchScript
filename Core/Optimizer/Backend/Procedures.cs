using Antlr4.Runtime.Atn;
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

        var procedure = new ScratchProcedure(name, arguments);
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

        foreach (var argument in context.callProcedureArgument())
        {
            var argumentName = argument.Identifier().GetText();
            var expression = Visit(argument.expression());
            if (argument.procedureArgumentType().GetText() == "i:")
                block.SetInput(procedure.Arguments[argumentName].Id, ScratchInput.New(expression, block));
            else 
                block.SetField(procedure.Arguments[argumentName].Id, ScratchField.New(expression));
            UpdateBlocks(expression);
        }
        
        return block;
    }

    public override object VisitRawCommand(ScratchIRParser.RawCommandContext context)
    {
        var opcode = context.Identifier().GetText();
        var block = new Block(opcode, "raw");

        foreach (var argument in context.callProcedureArgument())
        {
            var name = argument.Identifier().GetText();
            var expression = Visit(argument.expression());
            if (argument.procedureArgumentType().GetText() == ":i")
                block.SetInput(name, ScratchInput.New(expression, block));
            else 
                block.SetField(name, ScratchField.New(expression));
            if(expression is Block b) UpdateBlocks(b);
        }
        
        return block;
    }

    private void UpdateProcedure()
    {
        var procedure = _procedures.Last();
        var toUpdate = new List<object> { procedure.Definition, procedure.Prototype };
        toUpdate.AddRange(procedure.Arguments.Select(x => x.Value));
        UpdateBlocks(toUpdate.ToArray());
    }
}