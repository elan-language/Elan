namespace CSharpLanguageModel.Models;

public record QualifiedValueModel(ICodeModel Qualifier, ICodeModel Qualified) : ICodeModel {
    public string ToString(int indent) => $"{Qualifier}.{Qualified}";
    public override string ToString() => ToString(0);
}