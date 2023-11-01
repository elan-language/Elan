using AbstractSyntaxTree;

namespace CSharpLanguageModel.Models;

public record DefaultModel(ICodeModel Type, TypeType DefaultTypeType) : ICodeModel {
    public string ToString(int indent) => DefaultTypeType switch {
        TypeType.Value when Type is ScalarValueModel { Value: "string" } => @"""""",
        TypeType.Value => $@"default({Type})",
        TypeType.Class => $"{Type}.DefaultInstance",
        _ => throw new NotImplementedException()
    };

    public override string ToString() => ToString(0);
}