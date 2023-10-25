namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ClassDefModel(ICodeModel Type, IEnumerable<ICodeModel> Inherits, ICodeModel Constructor, IEnumerable<ICodeModel> Properties, IEnumerable<ICodeModel> Functions) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}public class {Type}{InheritsAsString()} {{
{ConstructorAsString(indent)}
{Properties.AsLineSeparatedString(indent + 1)}
{Functions.AsLineSeparatedString(indent + 1)}
{Indent(indent)}}}";

    private string ConstructorAsString(int indent) => $"{Indent(indent + 1)}public {Type}{Constructor.ToString(indent + 1)}";

    private string InheritsAsString() => Inherits.Any() ? $" : {Inherits.AsCommaSeparatedString()}" : "";

    public override string ToString() => ToString(0);
}