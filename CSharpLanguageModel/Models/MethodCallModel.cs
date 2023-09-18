namespace CSharpLanguageModel.Models;

public record MethodCallModel(ICodeModel Id, IEnumerable<ICodeModel> Parameters) : ICodeModel {
    public override string ToString() =>
        $@"
{Id}({Parameters.AsCommaSeparatedString()});
".Trim();
}