namespace CSharpLanguageModel.Models;

public record PropertyModel(ICodeModel Expression, ICodeModel Property) : ICodeModel {
    public string ToString(int indent) => $@"{Expression}.{Property}";
    public override string ToString() => ToString(0);
}