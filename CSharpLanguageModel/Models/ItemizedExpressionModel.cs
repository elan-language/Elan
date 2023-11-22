namespace CSharpLanguageModel.Models;

public record ItemizedExpressionModel(ICodeModel Expression, ICodeModel Range) : ICodeModel {
    private string Item => $"Item{int.Parse(Range.ToString() ?? "") + 1}";

    public string ToString(int indent) => $"{Expression}.{Item}";

    public override string ToString() => ToString(0);
}