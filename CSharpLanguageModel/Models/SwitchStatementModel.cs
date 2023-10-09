namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record SwitchStatementModel(ICodeModel Expression, IEnumerable<ICodeModel> ValueCases, ICodeModel? DefaultCase) : ICodeModel {
    public string ToString(int indent) {
        // check data 

        return $@"{Indent(indent)}switch ({Expression}) {{
{ValueCases.AsLineSeparatedString(indent + 1)}
{Indent(indent)}}}";
    }

}