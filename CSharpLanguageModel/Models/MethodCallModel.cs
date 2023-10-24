namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record MethodCallModel(MethodType MethodType, ICodeModel Id, ICodeModel? Qualifier, IEnumerable<ICodeModel> Parameters) : ICodeModel {


    private string Qual => Qualifier is null ? "" : $"{Qualifier}.";
    
    
    public string ToString(int indent) =>
        MethodType switch {
            MethodType.Function => $@"{Indent(indent)}{Qual}{Id}({Parameters.AsCommaSeparatedString()})",
            MethodType.SystemCall => $@"{Indent(indent)}{Qual}{Id}({Parameters.AsCommaSeparatedString()})",
            _ => throw new NotImplementedException()
        };

    public override string ToString() => ToString(0);
}