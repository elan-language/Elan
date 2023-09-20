namespace CSharpLanguageModel.Models;

public record IdentifierModel(string Id) : ICodeModel {

    public string ToString(int indent) => ToString();
    public override string ToString() => Id;
}