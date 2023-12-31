using Antlr4.Runtime;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private TypedValue? HandleListAssignment(ParserRuleContext context, TypedValue list, ref ScratchVariable variable)
    {
        if (!list.Data.ContainsKey("LIST_TYPE")) return null;

        Scope.Prepend += $"popall {variable.Id}\n";
        switch (list.Data["LIST_TYPE"])
        {
            case "ARRAY_EXPRESSION":
            {
                if (list.Value is not List<ScratchScriptParser.ExpressionContext> items || items.Count == 0)
                    return null;
                var type = ScratchType.Unknown;
                var push = "";
                foreach (var expression in items)
                {
                    var item = Visit(expression);
                    if (AssertNotNull(context, item, expression)) return null;
                    if (type == ScratchType.Unknown && item.Value.Type != ScratchType.Unknown)
                    {
                        type = item.Value.Type;
                        variable.Type = ScratchType.List(type);
                    }
                    else if (AssertType(context, item, type, expression)) return null;

                    push += $"push {variable.Id} {item.Format()}\n";
                }

                return new(push);
            }
        }

        return null;
    }

    private TypedValue? PackListAsArgument(TypedValue list)
    {
        if (!list.Data.ContainsKey("LIST_TYPE")) return null;
        switch (list.Data["LIST_TYPE"])
        {
            case "ARRAY_EXPRESSION":
            {
                if (list.Value is not List<ScratchScriptParser.ExpressionContext> items || items.Count == 0)
                    return null;
                var visitedItems = items.Select(x => (object)Visit(x)).ToArray();
                return Scope.PackList(visitedItems, TypeHelper.GetType(visitedItems.First()));
            }
        }

        return null;
    }

    public override TypedValue? VisitArrayAccessExpression(ScratchScriptParser.ArrayAccessExpressionContext context)
    {
        var obj = Visit(context.expression(0));
        var index = Visit(context.expression(1));
        if (AssertNotNull(context, obj, context.expression(0))) return null;
        if (AssertNotNull(context, index, context.expression(1))) return null;
        if (AssertType(context, index, ScratchType.Number, context.expression(1))) return null;

        var objectString = (string)obj.Value.Value;
        if (objectString.IsList())
        {
            //TODO: what is this
            return new($"{obj.Format().Replace("arr:", "")}#{index}", obj.Value.Type.ChildType);
        }

        if (obj.Value.Data.ContainsKey("ARGUMENT_NAME") && Scope.GetVariable(obj.Value.Data["ARGUMENT_NAME"]).Type.Kind == ScratchTypeKind.List)
            return Scope.CallFunction("__ReadListValue", new object[] { objectString, index },
                Scope.GetVariable(obj.Value.Data["ARGUMENT_NAME"]).Type.ChildType);

        if (obj.Value.Type == ScratchType.String)
        {
            var result = $"rawshadow operator_letter_of i:LETTER:{index} i:STRING:{obj} endshadow";
            return new(result, ScratchType.String);
        }

        return null;
    }

    public override TypedValue? VisitArrayInitializeExpression(
        ScratchScriptParser.ArrayInitializeExpressionContext context)
    {
        var type = ScratchType.Unknown;
        if (context.expression().Length != 0)
        {
            var firstValue = Visit(context.expression(0));
            if (firstValue.HasValue) type = firstValue.Value.Type;
        }
        return new(context.expression().ToList(), ScratchType.List(type),
            new Dictionary<string, string> { { "LIST_TYPE", "ARRAY_EXPRESSION" } });
    }
}