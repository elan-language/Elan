namespace CSharpLanguageModel.Models;

public record FuncTypeModel(IEnumerable<ICodeModel> Types, ICodeModel ReturnType) : ICodeModel {
    public string ToString(int indent) => $"Func<{Types.Append(ReturnType).AsCommaSeparatedString()}>";

    public override string ToString() => ToString(0);
}