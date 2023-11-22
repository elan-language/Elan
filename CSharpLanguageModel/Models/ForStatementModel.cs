namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ForStatementModel(ICodeModel Id, IEnumerable<ICodeModel> Expressions, ICodeModel? Step, bool Neg, ICodeModel StatementBlock) : ICodeModel {
    private string UpOrDown => Step is not null && Neg ? "-" : "+";
    private string GteOrLte => Step is not null && Neg ? ">=" : "<=";

    private string StepStr => Step is not null ? Step.ToString()! : "1";

    public string ToString(int indent) =>
        $@"{Indent(indent)}for (var {Id} = {Expressions.First()}; {Id} {GteOrLte} {Expressions.Last()}; {Id} = {Id} {UpOrDown} {StepStr}) {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}}";
}