namespace CSharpLanguageModel.Models;

public record ScalarValueModel(string Value) : ICodeModel {
    public override string ToString() => Value;
}