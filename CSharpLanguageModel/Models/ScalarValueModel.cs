namespace CSharpLanguageModel.Models;

public record ScalarValueModel(string Value) : ICodeModel {
    public string ToString(int indent) => MakeLiteralStringsInterpolated(Value);
    private static string MakeLiteralStringsInterpolated(string value) => value.StartsWith("\"") ? "@$" + value : value;
    public override string ToString() => ToString(0);
}