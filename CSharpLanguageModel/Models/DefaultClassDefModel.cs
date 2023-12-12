namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record DefaultClassDefModel(ICodeModel Type, ICodeModel[] Properties, ICodeModel[] Procedures, ICodeModel[] Functions, bool IsAbstract = false) : ICodeModel {
    private string Override => IsAbstract ? "" : " override";

    public string ToString(int indent) =>
        $@"{Indent(indent)}private record class _Default{Type} : {Type} {{
{DefaultConstructor(indent)}
{Properties.AsLineSeparatedString(indent + 1)}
{Procedures.AsLineSeparatedString($"public{Override} ", " { }", indent + 1)}{DefaultFunctions(Functions, indent + 1)}
{Indent(indent + 1)}public{Override} string asString() {{ return ""default {Type}"";  }}
{Indent(indent)}}}";

    private static string DefaultFunctions(ICodeModel[] mm, int indent = 0) {
        return string.Join("\r\n", mm.Select(cm => $"{Indent(indent)}public {cm}{Init(cm)}"));
    }

    private static string Init(ICodeModel cm) =>
        cm is MethodSignatureModel msm
            ? msm.ReturnType is IHasDefaultValue dm
                ? $" => {dm.DefaultValue}"
                : ";"
            : ";";

    private string DefaultConstructor(int indent) => $"{Indent(indent + 1)}public _Default{Type}() {{ }}";
}