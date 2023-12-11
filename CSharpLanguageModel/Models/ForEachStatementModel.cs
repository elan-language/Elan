namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ForEachStatementModel(ICodeModel Parameter, ICodeModel StatementBlock) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}foreach (var {Parameter}) {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}}";
}