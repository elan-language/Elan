namespace CSharpLanguageModel.Models;

public record DataStructureTypeModel(IEnumerable<ICodeModel> Items, string Type) : ICodeModel, IHasDefaultValue {
    public string ToString(int indent) => $"{Type}<{Items.AsCommaSeparatedString()}>";
    public string DefaultValue => Type.StartsWith("System") ? $"_DefaultIter<{Items.First()}>.DefaultInstance;" : $"{this}.DefaultInstance;";

    public override string ToString() => ToString(0);
}