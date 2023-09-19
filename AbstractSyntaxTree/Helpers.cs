namespace AbstractSyntaxTree;

public static class Helpers {
    public static Operator MapOperator(string token) =>
        token switch {
            "+" => Operator.Plus,
            "div" => Operator.Divide,
            "mod" => Operator.Mod,
            "^" => Operator.Power,
            _ => throw new NotSupportedException(token)
        };
}