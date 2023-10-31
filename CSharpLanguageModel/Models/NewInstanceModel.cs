namespace CSharpLanguageModel.Models;

public record NewInstanceModel(ICodeModel Type, IEnumerable<ICodeModel> Parameters, IEnumerable<ICodeModel> With) : ICodeModel {
    private string WithStr => With.Any() ? $" with {{{With.AsCommaSeparatedString()}}}" : "";

    public string ToString(int indent) => $@"new {Type}({Parameters.AsCommaSeparatedString()}){WithStr}";

    public override string ToString() => ToString(0);
}