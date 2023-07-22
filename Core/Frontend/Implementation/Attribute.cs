using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private void HandleProcedureAttribute(ScratchScriptParser.AttributeStatementContext context,
        ref ScratchIrProcedure procedure)
    {
        switch (context.Identifier().GetText())
        {
            case "__ReturnType":
            {
                var type = (ScratchType)Convert.ToInt32(Visit(context.constant(0)));
                procedure.ReturnType = type;
                return;
            }
            case "__ArgumentType":
            {
                var name = ((string)Visit(context.constant(0)))[1..^1];
                var type = (ScratchType)Convert.ToInt32(Visit(context.constant(1)));
                procedure.Arguments[name] = type;
                return;
            }
        }
    }
}