namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record SwitchStatementModel(ICodeModel Expression, IEnumerable<ICodeModel> ValueCases, ICodeModel? DefaultCase) : ICodeModel {

    private string DefCase(int indent) {
        return DefaultCase is null ? "" : $"\r\n{DefaultCase.ToString(indent)}";
    }

    public string ToString(int indent) {
        // check data 

        return $@"{Indent(indent)}switch ({Expression}) {{
{ValueCases.AsLineSeparatedString(indent + 1)}{DefCase(indent + 1)}
{Indent(indent)}}}";
    }

}