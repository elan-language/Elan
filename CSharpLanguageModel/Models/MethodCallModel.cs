namespace CSharpLanguageModel.Models;

public record MethodCallModel(ICodeModel Id, IEnumerable<ICodeModel> Parameters) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) =>
        $@"
{Id}({Parameters.AsCommaSeparatedString()});
".Trim();
}