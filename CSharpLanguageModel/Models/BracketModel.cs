namespace CSharpLanguageModel.Models;

public record BracketModel(ICodeModel Bracketed) : ICodeModel {
    public string ToString(int indent) => $"({Bracketed})";
    public override string ToString() => ToString(0);
}