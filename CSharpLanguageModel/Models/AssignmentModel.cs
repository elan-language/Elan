namespace CSharpLanguageModel.Models;

public record AssignmentModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) =>
        $@"
{Id} = {Expression};
".Trim();
}