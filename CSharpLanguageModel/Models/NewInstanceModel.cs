namespace CSharpLanguageModel.Models;

public record NewInstanceModel(ICodeModel Type, IEnumerable<ICodeModel> Parameters, IEnumerable<ICodeModel> Init) : ICodeModel {

    private string Initializer() => Init.Any() ? $" {{{Init.AsCommaSeparatedString()}}}" : "";
    
    
    public string ToString(int indent) => $@"new {Type}({Parameters.AsCommaSeparatedString()}){Initializer()}";

    public override string ToString() => ToString(0);
}