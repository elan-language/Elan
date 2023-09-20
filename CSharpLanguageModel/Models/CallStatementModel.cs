using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record CallStatementModel(ICodeModel CallModel) : ICodeModel {
    public override string ToString() => ToString(0);

    public string ToString(int indent) => $@"{CallModel.ToString(indent)};";
}