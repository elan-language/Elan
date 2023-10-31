namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record AssignmentModel(ICodeModel Id, ICodeModel Expression, bool Inline) : ICodeModel {

    private string Term => Inline ? "" : ";";

    public string ToString(int indent) => $@"{Indent(indent)}{Id} = {Expression}{Term}";

    public override string ToString() => ToString(0);
}