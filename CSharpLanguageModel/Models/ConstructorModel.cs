using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record ConstructorModel(IEnumerable<ICodeModel> Parameters, ICodeModel Statements) : ICodeModel {
    public override string ToString() => ToString(0);

    public string ToString(int indent) =>
        $@"({Parameters.AsCommaSeparatedString()}) {{
{Indent(Statements, indent + 1)}
{Indent(indent)}}}";
}