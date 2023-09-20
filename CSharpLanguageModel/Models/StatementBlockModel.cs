using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record StatementBlockModel(IEnumerable<ICodeModel> Statements) : ICodeModel {
    public override string ToString() => ToString(0);

    public string ToString(int indent) => $@"{Statements.AsLineSeparatedString(indent)}";
}