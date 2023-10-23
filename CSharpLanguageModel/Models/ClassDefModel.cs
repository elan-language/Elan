namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ClassDefModel(ICodeModel Type, ICodeModel Constructor, IEnumerable<ICodeModel> Properties) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}public class {Type} {{
{Indent(indent + 1)}public {Type}{Constructor.ToString(indent + 1)}
{Properties.AsLineSeparatedString(indent + 1)}
{Indent(indent)}}}";

    public override string ToString() => ToString(0);
}