using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;

namespace AbstractSyntaxTree;

public static class Helpers {
    public static Operator MapOperator(int nodeType) {
        return nodeType switch {
            ElanParser.PLUS => Operator.Plus,
            ElanParser.MINUS => Operator.Minus,
            ElanParser.MULT => Operator.Multiply,
            ElanParser.INT_DIV => Operator.IntDivide,
            ElanParser.MOD => Operator.Modulus,
            ElanParser.POWER => Operator.Power,
            ElanParser.DIVIDE => Operator.Divide,
            ElanParser.OP_EQ => Operator.Equal,
            ElanParser.OP_AND => Operator.And,
            ElanParser.OP_OR => Operator.Or,
            ElanParser.OP_NOT => Operator.Not,
            ElanParser.OP_XOR => Operator.Xor,
            ElanParser.LT => Operator.LessThan,
            ElanParser.GT => Operator.GreaterThan,
            ElanParser.OP_GE => Operator.GreaterThanEqual,
            ElanParser.OP_LE => Operator.LessThanEqual,
            ElanParser.OP_NE => Operator.NotEqual,
            _ => throw new NotSupportedException(nodeType.ToString())
        };
    }

    public static ValueType MapValueType(string type) {
        return type switch {
            "String" => ValueType.String,
            "Int" => ValueType.Int,
            _ => throw new NotSupportedException(type)
        };
    }

    public static ImmutableArray<IAstNode> SafeReplace(this ImmutableArray<IAstNode> nodes, IAstNode oldValue, IAstNode newValue) =>
        nodes.Contains(oldValue) ? nodes.Replace(oldValue, newValue) : nodes;
}