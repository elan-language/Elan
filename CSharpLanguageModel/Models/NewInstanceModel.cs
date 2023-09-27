namespace CSharpLanguageModel.Models;

public record NewInstanceModel(ICodeModel Type) : ICodeModel {
    public string ToString(int indent) => $@"new {Type}";

    public override string ToString() => ToString(0);
}