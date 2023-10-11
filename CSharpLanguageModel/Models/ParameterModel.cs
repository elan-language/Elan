namespace CSharpLanguageModel.Models;

public record ParameterModel(ICodeModel Id, ICodeModel Type) : ICodeModel {
    public string ToString(int indent) => $@"{Type} {Id}";

    public override string ToString() => ToString(0);
}