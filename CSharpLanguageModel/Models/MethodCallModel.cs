namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record MethodCallModel(ICodeModel Id, IEnumerable<ICodeModel> Parameters) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) =>
        $@"{Indent(indent)}{Id}({Parameters.AsCommaSeparatedString()})";
}