namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record AbstractClassDefModel(ICodeModel Type, ICodeModel[] Inherits, ICodeModel[] Properties, ICodeModel[] Functions, ICodeModel DefaultClassModel) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}public interface {Type}{InheritsAsString()} {{
{CodeHelpers.DefaultInstance(indent, Type)}
{Properties.AsLineSeparatedString(indent + 1)}
{Functions.AsLineSeparatedString("public ", ";", indent + 1)}
{DefaultClassModel.ToString(indent + 1)}
{Indent(indent)}}}";

    private string InheritsAsString() => Inherits.Any() ? $" : {Inherits.AsCommaSeparatedString()}" : "";
}