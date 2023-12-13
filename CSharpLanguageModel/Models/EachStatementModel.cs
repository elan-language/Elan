namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record EachStatementModel(ICodeModel Parameter, ICodeModel StatementBlock) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}foreach (var {Parameter}) {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}}";
}