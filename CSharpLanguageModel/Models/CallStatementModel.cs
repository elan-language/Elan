namespace CSharpLanguageModel.Models;

public record CallStatementModel(ICodeModel CallModel) : ICodeModel {
    public string ToString(int indent) => $@"{CallModel.ToString(indent)};";
    public override string ToString() => ToString(0);
}