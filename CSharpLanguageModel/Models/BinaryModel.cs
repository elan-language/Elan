namespace CSharpLanguageModel.Models;

public record BinaryModel(ICodeModel Operator, ICodeModel Operand1, ICodeModel Operand2) : ICodeModel {

    public string ToString(int indent) => ToString();
    private bool IsOperator => Operator is ScalarValueModel;

    private string ToOperatorString() =>
        $@"
{Operand1} {Operator} {Operand2}
".Trim();

    // To handle case where C# doesn't support power operator 
    private string ToFunctionString() =>
        $@"
{Operator}({Operand1}, {Operand2})
".Trim();

    public override string ToString() => IsOperator ? ToOperatorString() : ToFunctionString();
}