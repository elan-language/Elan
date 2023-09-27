namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record IndexedExpressionModel(ICodeModel Expression, ICodeModel Range) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) => $@"{Expression}[{Range}]";
}