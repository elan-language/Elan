namespace CSharpLanguageModel.Models;

public record DeconstructionModel(IEnumerable<ICodeModel> With) : ICodeModel {
    public string ToString(int indent) =>$"({With.AsCommaSeparatedString()})";

    public override string ToString() => ToString(0);
}