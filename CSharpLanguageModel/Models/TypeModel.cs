namespace CSharpLanguageModel.Models;

public record TypeModel(ICodeModel Id) : ICodeModel, IHasDefaultValue {
    public string ToString(int indent) => $"{Id}";
    public string DefaultValue => $"{this}.DefaultInstance;";
    public override string ToString() => ToString(0);
}