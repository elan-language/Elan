namespace CSharpLanguageModel.Models;

public record TypeModel(ICodeModel Id) : ICodeModel {
    public string ToString(int indent) => $@"{Id}";

    public override string ToString() => ToString(0);
}