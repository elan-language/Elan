namespace CSharpLanguageModel.Models;

public record MainCodeModel(IEnumerable<ICodeModel> Statements) : ICodeModel {
    public override string ToString() =>
        $@"public static class Program {{
    private static void Main(string[] args) {{
      {Statements.AsLineSeparatedString()}
    }}
}}".Trim();
}