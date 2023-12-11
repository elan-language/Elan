namespace CSharpLanguageModel.Models;

public record IfExpressionModel(ICodeModel[] Expressions) : ICodeModel {
    public string ToString(int indent) {
        if (Expressions.Count() is 3) {
            var expr1 = Expressions.First();
            var expr2 = Expressions.Skip(1).First();
            var expr3 = Expressions.Last();

            return $"{expr1} ? {expr2} : {expr3}";
        }

        throw new CodeGenerationException("Mismatched count if expressions/blocks in If");
    }

    public override string ToString() => ToString(0);
}