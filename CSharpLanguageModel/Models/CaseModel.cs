namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record CaseModel(ICodeModel? Value, ICodeModel StatementBlock) : ICodeModel {
    private string Case => Value is not null ? $"case {Value}" : "default";

    public string ToString(int indent) => $@"{Indent(indent)}{Case}:
{Indent(indent + 1)}{StatementBlock}
{Indent(indent + 1)}break;";

    public override string ToString() => ToString(0);
}