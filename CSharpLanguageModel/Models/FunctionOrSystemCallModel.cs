namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record FunctionOrSystemCallModel(ICodeModel Id, ICodeModel? Qualifier, ICodeModel[] Parameters) : ICodeModel {
    private string Qual => Qualifier is null ? "" : $"{Qualifier}.";

    public string ToString(int indent) => $"{Indent(indent)}{Qual}{Id}({Parameters.AsCommaSeparatedString()})";

    public override string ToString() => ToString(0);
}