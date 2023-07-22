using ScratchScript.Core.Blocks;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Optimizer.Backend;

public partial class ScratchIRBackendVisitor
{
    public override object VisitNotExpression(ScratchIRParser.NotExpressionContext context)
    {
        var expression = Visit(context.expression());

        var block = Operators.Not();
        block.SetInput("OPERAND", ScratchInput.New(expression, block));
        UpdateBlocks(block, expression);

        return block;
    }
}