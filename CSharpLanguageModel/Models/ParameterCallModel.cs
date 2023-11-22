namespace CSharpLanguageModel.Models;

public record ParameterCallModel(ICodeModel Expression, bool IsRef = false) : ICodeModel {
    private string IsRefStr => IsRef ? "ref " : "";

    public string ToString(int indent) => $"{IsRefStr}{Expression}";
}