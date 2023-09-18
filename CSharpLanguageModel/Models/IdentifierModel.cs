namespace CSharpLanguageModel.Models;

public record IdentifierModel(string Id) : ICodeModel {
    public override string ToString() => Id;
}