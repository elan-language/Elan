namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record WhileStatementModel(ICodeModel Expression, ICodeModel StatementBlock) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}while ({Expression}) {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}}";
}