using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record BracketModel(ICodeModel Bracketed) : ICodeModel {
    public override string ToString() => ToString(0);

    public string ToString(int indent) => $@"({Bracketed})";
}