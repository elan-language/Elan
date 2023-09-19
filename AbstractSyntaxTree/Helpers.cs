namespace AbstractSyntaxTree;

public static class Helpers {
    public static Operator MapOperator(int nodeType) {
        return nodeType switch {
            ElanParser.PLUS => Operator.Plus,
            ElanParser.INT_DIV => Operator.IntDiv,
            ElanParser.MOD => Operator.Mod,
            ElanParser.POWER => Operator.Power,
            ElanParser.DIVIDE => Operator.Divide,
            _ => throw new NotSupportedException(nodeType.ToString())
        };
    }
}