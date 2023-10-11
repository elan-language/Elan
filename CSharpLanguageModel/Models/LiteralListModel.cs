namespace CSharpLanguageModel.Models;

public record LiteralListModel(IEnumerable<ICodeModel> Items, string Type) : ICodeModel {
    public string ToString(int indent) => $@"new StandardLibrary.ElanList<{Type}>({Items.AsCommaSeparatedString()})";

    public override string ToString() => ToString(0);
}