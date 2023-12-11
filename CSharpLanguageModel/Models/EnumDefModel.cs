namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record EnumDefModel(ICodeModel Type, ICodeModel[] Values) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}public enum {Type} {{
{Values.AsLineSeparatedString(",", indent + 1)}
{Indent(indent)}}}";
}