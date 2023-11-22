namespace CSharpLanguageModel.Models;

public record IndexedExpressionModel(ICodeModel Expression, ICodeModel Range) : ICodeModel {
    public string ToString(int indent) => $"{Expression}[{Range}]";

    public override string ToString() => ToString(0);
}