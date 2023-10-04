namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record MethodCallModel(MethodType MethodType, ICodeModel Id, IEnumerable<ICodeModel> Parameters) : ICodeModel {
    private string Prefix => "ref";

    public string ToString(int indent) =>
        MethodType switch {
            MethodType.Procedure => $@"{Indent(indent)}{Id}({Parameters.AsCommaSeparatedString(Prefix)})",
            MethodType.Function => $@"{Indent(indent)}{Id}({Parameters.AsCommaSeparatedString()})",
            MethodType.SystemCall => $@"{Indent(indent)}{Id}({Parameters.AsCommaSeparatedString()})",
            _ => throw new NotImplementedException()
        };

    public override string ToString() => ToString(0);
}