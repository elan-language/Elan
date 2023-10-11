namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ForInStatementModel(ICodeModel Id, ICodeModel Expression, ICodeModel StatementBlock) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}foreach (var {Id} in {Expression}) {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}}";

    public override string ToString() => ToString(0);
}