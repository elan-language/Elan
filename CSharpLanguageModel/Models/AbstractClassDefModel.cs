namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record AbstractClassDefModel(ICodeModel Type, IEnumerable<ICodeModel> Inherits, IEnumerable<ICodeModel> Properties, IEnumerable<ICodeModel> Functions) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}public interface {Type}{InheritsAsString()} {{
{Properties.AsLineSeparatedString(indent + 1)}
{Functions.AsLineSeparatedString("public ", ";", indent + 1)}
{Indent(indent)}}}";

    private string InheritsAsString() => Inherits.Any() ? $" : {Inherits.AsCommaSeparatedString()}" : "";

    public override string ToString() => ToString(0);
}