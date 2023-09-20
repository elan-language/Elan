namespace CSharpLanguageModel.Models;

public record IdentifierModel(string Id) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) => Id;
}