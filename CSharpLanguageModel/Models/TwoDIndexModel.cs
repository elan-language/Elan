namespace CSharpLanguageModel.Models;

public record TwoDIndexModel(ICodeModel Index1, ICodeModel Index2) : ICodeModel {
    public string ToString(int indent) => $"{Index1},{Index2}";

    public override string ToString() => ToString(0);
}