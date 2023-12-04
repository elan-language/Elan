namespace CSharpLanguageModel.Models;

public record ScalarValueModel(string Value) : ICodeModel, IHasDefaultValue {

    public string DefaultValue => Value is "string" ? @""""";" : "default;";

    public string ToString(int indent) => MakeLiteralStringsInterpolated(Value);
    private static string MakeLiteralStringsInterpolated(string value) => value.StartsWith("\"") ? "@$" + PrefixKeywords(value) : HandleEmptyChar(value);

    private static string PrefixKeywords(string value) => CodeHelpers.CSharpKeywordRegex.Replace(value, "{@$1}");

    private static string HandleEmptyChar(string value) => value is "''" ? "default(char)" : value;


    public override string ToString() => ToString(0);
}