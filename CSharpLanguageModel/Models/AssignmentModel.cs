namespace CSharpLanguageModel.Models;

public record AssignmentModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {

    public string ToString(int indent) => ToString();
    public override string ToString() =>
        $@"
{Id} = {Expression};
".Trim();
}