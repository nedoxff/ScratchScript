using ScratchScript.Core.Blocks;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Optimizer.Backend;

public partial class ScratchIRBackendVisitor
{
    public override object VisitAddOperators(ScratchIRParser.AddOperatorsContext context)
    {
        return context.GetText() switch
        {
            "+" => Operators.Add(),
            "-" => Operators.Subtract(),
            "~" => Operators.Join(),
            _ => null
        };
    }

    public override object VisitMultiplyOperators(ScratchIRParser.MultiplyOperatorsContext context)
    {
        return context.GetText() switch
        {
            "*" => Operators.Multiply(),
            "/" => Operators.Divide(),
            "%" => Operators.Modulus(),
            _ => null
        };
    }

    public override object VisitBooleanOperators(ScratchIRParser.BooleanOperatorsContext context)
    {
        return context.GetText() switch
        {
            "&&" => Operators.And(),
            "||" => Operators.Or(),
            _ => null
        };
    }

    public override object VisitCompareOperators(ScratchIRParser.CompareOperatorsContext context)
    {
        return context.GetText() switch
        {
            "<" => Operators.LessThan(),
            ">" => Operators.GreaterThan(),
            "==" => Operators.Equals(),
            "!=" => Not(),
            ">=" => GreaterOrEqualTo(),
            "<=" => LessOrEqualTo(),
            _ => null
        };
    }

    private Block GreaterOrEqualTo()
    {
        var or = Operators.Or();
        var greater = Operators.GreaterThan();
        var equal = Operators.Equals();
        or.SetInput("OPERAND1", ScratchInput.New(greater, or));
        or.SetInput("OPERAND2", ScratchInput.New(equal, or));
        or.CustomData["COMPLEX_OPERATOR"] = "true";
        or.CustomData["COMPLEX_FIRST"] = greater.Id;
        or.CustomData["COMPLEX_SECOND"] = equal.Id;
        UpdateBlocks(or, greater, equal);
        return or;
    }

    private Block LessOrEqualTo()
    {
        var or = Operators.Or();
        var less = Operators.LessThan();
        var equal = Operators.Equals();
        or.SetInput("OPERAND1", ScratchInput.New(less, or));
        or.SetInput("OPERAND2", ScratchInput.New(equal, or));
        or.CustomData["COMPLEX_OPERATOR"] = "true";
        or.CustomData["COMPLEX_FIRST"] = less.Id;
        or.CustomData["COMPLEX_SECOND"] = equal.Id;
        UpdateBlocks(or, less, equal);
        return or;
    }

    private Block Not()
    {
        var not = Operators.Not();
        var equal = Operators.Equals();
        not.SetInput("OPERAND", ScratchInput.New(equal, not));
        not.CustomData["BINARY_BLOCK"] = equal.Id;
        not.CustomData["COMPLEX_OPERATOR"] = "true";
        UpdateBlocks(not, equal);
        return not;
    }
}