namespace CSharpLanguageModel.Models;

public record UnaryModel(ICodeModel Operator, ICodeModel Operand) : ICodeModel {
    public string ToString(int indent) => $@"{Operator}{Operand}";

    public override string ToString() => ToString(0);
}