namespace CSharpLanguageModel.Models;

public record AssignmentModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {
    public override string ToString() =>
        $@"
{Id} = {Expression};
".Trim();
}