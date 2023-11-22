namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ProcedureCallModel(ICodeModel Id, ICodeModel? Qualifier, IEnumerable<ICodeModel> Parameters) : ICodeModel {
    private string Qual => Qualifier is null ? "" : $"{Qualifier}.";

    public string ToString(int indent) => $@"{Indent(indent)}{Qual}{Id}({Parameters.AsCommaSeparatedString()})";
}