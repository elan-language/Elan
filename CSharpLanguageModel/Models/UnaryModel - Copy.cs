namespace CSharpLanguageModel.Models;

public record EnumValueModel(string Value, ICodeModel Type) : ICodeModel {
    public string ToString(int indent) => $@"{Type}.{Value}";

    public override string ToString() => ToString(0);
}