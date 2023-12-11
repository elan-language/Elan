namespace CSharpLanguageModel.Models;

public record TupleModel(ICodeModel[] Items) : ICodeModel {
    public string ToString(int indent) => $"({Items.AsCommaSeparatedString()})";

    public override string ToString() => ToString(0);
}