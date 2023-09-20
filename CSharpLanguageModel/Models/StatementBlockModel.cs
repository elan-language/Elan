using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record StatementBlockModel(IEnumerable<ICodeModel> Statements) : ICodeModel {

    public string ToString(int indent) => $@"{Statements.AsLineSeparatedString(indent)}";
}