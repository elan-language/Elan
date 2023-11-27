namespace CSharpLanguageModel.Models;

public record DataStructureTypeModel(IEnumerable<ICodeModel> Items, string Type) : ICodeModel {
    public string ToString(int indent) => $"{Type}<{Items.AsCommaSeparatedString()}>";

    public override string ToString() => ToString(0);
}