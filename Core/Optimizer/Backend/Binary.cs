using System.Xml.Serialization;
using Antlr4.Runtime.Tree;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Optimizer.Backend;

public partial class ScratchIRBackendVisitor
{
    public override object VisitBinaryAddExpression(ScratchIRParser.BinaryAddExpressionContext context)
    {
        var @operator = Visit(context.addOperators()) as Block;
        return GenericBinaryMathematicalExpression(@operator, context.expression(0), context.expression(1));
    }
    
    public override object VisitBinaryMultiplyExpression(ScratchIRParser.BinaryMultiplyExpressionContext context)
    {
        var @operator = Visit(context.multiplyOperators()) as Block;
        return GenericBinaryMathematicalExpression(@operator, context.expression(0), context.expression(1));
    }

    public override object VisitBinaryBooleanExpression(ScratchIRParser.BinaryBooleanExpressionContext context)
    {
        var @operator = Visit(context.booleanOperators()) as Block;
        return GenericBinaryConditionalExpression(@operator, context.expression(0), context.expression(1));
    }

    public override object VisitBinaryCompareExpression(ScratchIRParser.BinaryCompareExpressionContext context)
    {
        var @operator = Visit(context.compareOperators()) as Block;
        return GenericBinaryConditionalExpression(@operator, context.expression(0), context.expression(1));
    }

    private Block GenericBinaryMathematicalExpression(Block @operator, IParseTree first, IParseTree second)
    {
        var firstResult = Visit(first);
        var secondResult = Visit(second);

        var firstInput = @operator.Opcode == "operator_join" ? "STRING1" : "NUM1";
        var secondInput = @operator.Opcode == "operator_join" ? "STRING2" : "NUM2";
        @operator.SetInput(firstInput, ScratchInput.New(firstResult, @operator));
        @operator.SetInput(secondInput, ScratchInput.New(secondResult, @operator));

        UpdateBlocks(@operator, firstResult, secondResult);
        return @operator;
    }

    private Block GenericBinaryConditionalExpression(Block @operator, IParseTree first, IParseTree second)
    {
        var firstResult = Visit(first);
        var secondResult = Visit(second);
        
        if(@operator.CustomData.ContainsKey("COMPLEX_OPERATOR"))
            HandleComplexOperator(@operator, firstResult, secondResult);
        else
        {
            @operator.SetInput("OPERAND1", ScratchInput.New(firstResult, @operator));
            @operator.SetInput("OPERAND2", ScratchInput.New(secondResult, @operator));   
        }

        UpdateBlocks(@operator, firstResult, secondResult);
        return @operator;
    }

    private void HandleComplexOperator(Block @operator, object first, object second)
    {
        switch (@operator.Opcode)
        {
            case "operator_not":
            {
                var block = Blocks[@operator.CustomData["BINARY_BLOCK"]];
                block.SetInput("OPERAND1", ScratchInput.New(first, block));
                block.SetInput("OPERAND2", ScratchInput.New(second, block));
                UpdateBlocks(block);
                break;
            }
            default:
            {
                var firstCondition = Blocks[@operator.CustomData["COMPLEX_FIRST"]];
                var secondCondition = Blocks[@operator.CustomData["COMPLEX_SECOND"]];

                var firstCloned = CloneIfBlock(first);
                var secondCloned = CloneIfBlock(second);
                
                firstCondition.SetInput("OPERAND1", ScratchInput.New(first, firstCondition));
                firstCondition.SetInput("OPERAND2", ScratchInput.New(second, firstCondition));
                secondCondition.SetInput("OPERAND1", ScratchInput.New(firstCloned, secondCondition));
                secondCondition.SetInput("OPERAND2", ScratchInput.New(secondCloned, secondCondition));
                
                UpdateBlocks(firstCondition, secondCondition, first, second, secondCloned, secondCloned);
                break;
            }
        }
    }

    private object CloneIfBlock(object o) => o is Block b ? b.Clone() : o;
}