namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ClassDefModel(ICodeModel Type, IEnumerable<ICodeModel> Inherits, ICodeModel Constructor, IEnumerable<ICodeModel> Properties, IEnumerable<ICodeModel> Functions, bool HasDefaultConstructor, ICodeModel DefaultClassModel) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}public record class {Type}{InheritsAsString()} {{
{DefaultInstance(indent)}
{DefaultConstructor(indent)}
{ConstructorAsString(indent)}
{Properties.AsLineSeparatedString(indent + 1)}
{Functions.AsLineSeparatedString(indent + 1)}
{DefaultClassModel.ToString(indent + 1)}
{Indent(indent)}}}";

    private string ConstructorAsString(int indent) => $"{Indent(indent + 1)}public {Type}{Constructor.ToString(indent + 1)}";

    private string DefaultConstructor(int indent) => HasDefaultConstructor ? "" : $"{Indent(indent + 1)}private {Type}() {{}}";

    private string DefaultInstance(int indent) => $"{Indent(indent + 1)}public static {Type} DefaultInstance {{ get; }} = new {Type}._Default{Type}();";

    private string InheritsAsString() => Inherits.Any() ? $" : {Inherits.AsCommaSeparatedString()}" : "";

    public override string ToString() => ToString(0);
}