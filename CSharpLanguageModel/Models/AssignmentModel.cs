namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record AssignmentModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {
    public string ToString(int indent) => $@"{Indent(indent)}{Id} = {Expression};";

    public override string ToString() => ToString(0);
}