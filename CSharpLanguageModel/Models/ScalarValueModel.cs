namespace CSharpLanguageModel.Models;

public record ScalarValueModel(string Value) : ICodeModel {

    public string ToString(int indent) => Value;
    public override string ToString() => ToString(0);
}