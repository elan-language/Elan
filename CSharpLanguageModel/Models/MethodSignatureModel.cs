namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record MethodSignatureModel(ICodeModel Id, IEnumerable<ICodeModel> Parameters) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) =>
        $@"public static void {Id}({Parameters.AsCommaSeparatedString("ref")})";
}