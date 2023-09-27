namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ForStatementModel(ICodeModel Id, IEnumerable<ICodeModel> Expressions, ICodeModel? Step, ICodeModel StatementBlock) : ICodeModel {
    private string GteOrLte { get; set; } = "<=";

    private string StepStr { get; set; } = "1";

    public string ToString(int indent) {
        Init();
        return $@"{Indent(indent)}for (var {Id} = {Expressions.First()}; {Id} {GteOrLte} {Expressions.Last()}; {Id} = {Id} + {StepStr}) {{
{StatementBlock.ToString(indent + 1)}
{Indent(indent)}}}";
    }

    private void Init() {
        if (Step is not null) {
            if (int.TryParse(Step.ToString(), out var si)) {
                if (si < 0) {
                    GteOrLte = ">=";
                }

                StepStr = si.ToString();
            }
        }
    }

    public override string ToString() => ToString(0);
}