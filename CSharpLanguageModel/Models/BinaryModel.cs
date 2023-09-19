namespace CSharpLanguageModel.Models;

public record BinaryModel(ICodeModel Operator, ICodeModel Operand1, ICodeModel Operand2) : ICodeModel {
    public override string ToString() =>
        $@"
{Operand1} {Operator} {Operand2}
".Trim();
}