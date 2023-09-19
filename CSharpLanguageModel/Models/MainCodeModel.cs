using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record MainCodeModel(IEnumerable<ICodeModel> Statements) : ICodeModel {
    public override string ToString() =>
        $@"
public static class Program {{
{Indent1}private static void Main(string[] args) {{
{Statements.AsLineSeparatedString(2)}
{Indent1}}}
}}".Trim();
}