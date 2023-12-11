namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ClassDefModel(ICodeModel Type, ICodeModel[] Inherits, ICodeModel Constructor, ICodeModel[] Properties, ICodeModel[] Functions, bool HasDefaultConstructor, ICodeModel DefaultClassModel) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}public record class {Type}{InheritsAsString()} {{
{CodeHelpers.DefaultInstance(indent, Type)}
{DefaultConstructor(indent)}
{ConstructorAsString(indent)}
{Properties.AsLineSeparatedString(indent + 1)}
{Functions.AsLineSeparatedString(indent + 1)}
{DefaultClassModel.ToString(indent + 1)}
{Indent(indent)}}}";

    private string ConstructorAsString(int indent) => $"{Indent(indent + 1)}public {Type}{Constructor.ToString(indent + 1)}";

    private string DefaultConstructor(int indent) => HasDefaultConstructor ? "" : $"{Indent(indent + 1)}private {Type}() {{}}";

    private string InheritsAsString() => Inherits.Any() ? $" : {Inherits.AsCommaSeparatedString()}" : "";
}