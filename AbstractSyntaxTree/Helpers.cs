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
            _ => throw new NotSupportedException(nodeType.ToString())
        };
    }
}