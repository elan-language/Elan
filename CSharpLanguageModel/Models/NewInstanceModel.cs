namespace CSharpLanguageModel.Models;

public record NewInstanceModel(ICodeModel Type, ICodeModel[] Parameters) : ICodeModel {
    public string ToString(int indent) => $"new {Type}({Parameters.AsCommaSeparatedString()})";

    public override string ToString() => ToString(0);
}