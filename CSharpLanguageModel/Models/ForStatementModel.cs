namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ForStatementModel(ICodeModel Id, IEnumerable<ICodeModel> Expressions, ICodeModel StatementBlock) : ICodeModel {
    private string Step => Expressions.Count() == 3 ? $"{Expressions.Last()}" : "1";

    public string ToString(int indent) =>
        $@"{Indent(indent)}for (var {Id} = {Expressions.First()}; {Id} <= {Expressions.Skip(1).First()}; {Id} = {Id} + {Step}) {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}}";

    public override string ToString() => ToString(0);
}