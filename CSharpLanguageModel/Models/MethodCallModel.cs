namespace CSharpLanguageModel.Models;

public record MethodCallModel(ICodeModel Id, IEnumerable<ICodeModel> Parameters) : ICodeModel {

    public string ToString(int indent) => ToString();
    public override string ToString() =>
        $@"
{Id}({Parameters.AsCommaSeparatedString()});
".Trim();
}