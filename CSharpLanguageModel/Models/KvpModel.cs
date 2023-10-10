namespace CSharpLanguageModel.Models;

public record KvpModel(ICodeModel Key, ICodeModel Value) : ICodeModel {
    public string ToString(int indent) => $@"KeyValuePair.Create({Key}, {Value})";

    public override string ToString() => ToString(0);
}

