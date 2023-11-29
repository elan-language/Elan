namespace CSharpLanguageModel.Models;

public record TupleModel(IEnumerable<ICodeModel> Items) : ICodeModel {
    public string ToString(int indent) => $"({Items.AsCommaSeparatedString()})";

    public override string ToString() => ToString(0);
}