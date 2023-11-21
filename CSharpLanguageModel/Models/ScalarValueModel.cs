namespace CSharpLanguageModel.Models;

public record ScalarValueModel(string Value) : ICodeModel {
    public string ToString(int indent) => MakeLiteralStringsInterpolated(Value);
    private static string MakeLiteralStringsInterpolated(string value) => value.StartsWith("\"") ? "@$" + PrefixKeywords(value) : value;

    private static string PrefixKeywords(string value) => CodeHelpers.CSharpKeywordRegex.Replace(value, "{@$1}");

    public override string ToString() => ToString(0);
}