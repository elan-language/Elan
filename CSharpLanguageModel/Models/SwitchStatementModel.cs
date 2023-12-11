namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record SwitchStatementModel(ICodeModel Expression, ICodeModel[] ValueCases, ICodeModel DefaultCase) : ICodeModel {
    public string ToString(int indent) {
        // check data 

        var indent1 = indent + 1;
        return $@"{Indent(indent)}switch ({Expression}) {{
{ValueCases.AsLineSeparatedString(indent + 1)}
{DefaultCase.ToString(indent1)}
{Indent(indent)}}}";
    }
}