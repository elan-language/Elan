namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record MethodSignatureModel(ICodeModel Id, IEnumerable<ICodeModel> Parameters, ICodeModel? ReturnType) : ICodeModel {
    private string ReturnTypeString => ReturnType is not null ? $"{ReturnType}" : "void";

    private string Prefix => ReturnType is not null ? "" : "ref ";

    public string ToString(int indent) =>
        $@"{Indent(indent)}{ReturnTypeString} {Id}({Parameters.AsCommaSeparatedString(Prefix)})";

    public override string ToString() => ToString(0);
}