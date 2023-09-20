namespace CSharpLanguageModel.Models;

public record BinaryModel(ICodeModel Operator, ICodeModel Operand1, ICodeModel Operand2) : ICodeModel {
    private bool IsOperator => Operator is ScalarValueModel;

    public string ToString(int indent) => IsOperator ? ToOperatorString() : ToFunctionString();

    public override string ToString() => ToString(0);

    private string ToOperatorString() => $@"{Operand1} {Operator} {Operand2}";

    // To handle case where C# doesn't support power operator 
    private string ToFunctionString() => $@"{Operator}({Operand1}, {Operand2})";
}