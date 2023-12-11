using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;

namespace AbstractSyntaxTree;

public static class Helpers {
    public static Operator MapOperator(int nodeType) {
        return nodeType switch {
            ElanParser.PLUS => Operator.Plus,
            ElanParser.MINUS => Operator.Minus,
            ElanParser.MULT => Operator.Multiply,
            ElanParser.DIV => Operator.IntDivide,
            ElanParser.MOD => Operator.Modulus,
            ElanParser.POWER => Operator.Power,
            ElanParser.DIVIDE => Operator.Divide,
            ElanParser.IS => Operator.Equal,
            ElanParser.AND => Operator.And,
            ElanParser.OR => Operator.Or,
            ElanParser.NOT => Operator.Not,
            ElanParser.XOR => Operator.Xor,
            ElanParser.LT => Operator.LessThan,
            ElanParser.GT => Operator.GreaterThan,
            ElanParser.GE => Operator.GreaterThanEqual,
            ElanParser.LE => Operator.LessThanEqual,
            ElanParser.IS_NOT => Operator.NotEqual,
            _ => throw new NotSupportedException(nodeType.ToString())
        };
    }

    public static ValueType MapValueType(string type) {
        return type switch {
            "String" => ValueType.String,
            "Int" => ValueType.Int,
            "Float" => ValueType.Float,
            "Char" => ValueType.Char,
            "Bool" => ValueType.Bool,
            _ => throw new NotSupportedException(type)
        };
    }

    public static ImmutableArray<IAstNode> SafeReplace(this ImmutableArray<IAstNode> nodes, IAstNode oldValue, IAstNode newValue) =>
        nodes.Contains(oldValue) ? nodes.Replace(oldValue, newValue) : nodes;

    public static IEnumerable<IAstNode> SafeAppend(this IEnumerable<IAstNode> nodes, IAstNode? node) =>
        node is null ? nodes : nodes.Append(node);

    public static IEnumerable<IAstNode> SafePrepend(this IEnumerable<IAstNode> nodes, IAstNode? node) =>
        node is null ? nodes : nodes.Prepend(node);

    public static string UniqueID => Guid.NewGuid().ToString();

    public static string UniqueLambdaName => $"_lambda{UniqueID}";

    public static string UniqueScopeName => $"_scope{UniqueID}";
}