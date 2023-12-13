namespace CSharpLanguageModel.Models;

public record EachParameterModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {
    public string ToString(int indent) => $"{Id} in {Expression}";

    public override string ToString() => ToString(0);
}