using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

// TODO change to statement block
public record MainCodeModel(ICodeModel Statements) : ICodeModel {

    public string ToString(int indent) => ToString();

    public override string ToString() =>
        $@"public static class Program {{
{Indent1}private static void Main(string[] args) {{
{Indent(Statements, 2)}
{Indent1}}}
}}";
}