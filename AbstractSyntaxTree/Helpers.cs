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
            _ => throw new NotSupportedException(nodeType.ToString())
        };
    }
}