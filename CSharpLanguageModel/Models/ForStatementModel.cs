namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ForStatementModel(ICodeModel Id, IEnumerable<ICodeModel> Expressions, ICodeModel StatementBlock) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}for (var {Id} = {Expressions.First()}; {Id} <= {Expressions.Last()}; {Id} = {Id} + 1) {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}}";

    public override string ToString() => ToString(0);
}