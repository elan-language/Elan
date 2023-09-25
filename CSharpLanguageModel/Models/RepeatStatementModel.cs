namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record RepeatStatementModel(ICodeModel Expression, ICodeModel StatementBlock) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}do {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}} while (!({Expression}));";
}