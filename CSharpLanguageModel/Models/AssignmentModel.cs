namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record AssignmentModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) => $@"{Indent(indent)}{Id} = {Expression};";
}