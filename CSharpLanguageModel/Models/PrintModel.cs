namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record PrintModel(ICodeModel Expression) : ICodeModel {
    public string ToString(int indent) => $@"{Indent(indent)}print({Expression});";
    public override string ToString() => ToString(0);
}
