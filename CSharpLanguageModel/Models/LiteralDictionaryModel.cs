namespace CSharpLanguageModel.Models;

public record LiteralDictionaryModel(ICodeModel[] Items, string Type) : ICodeModel {
    public string ToString(int indent) => $"new StandardLibrary.ElanDictionary<{Type}>({Items.AsCommaSeparatedString()})";

    public override string ToString() => ToString(0);
}