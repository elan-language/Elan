namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ClassDefModel(ICodeModel Type, ICodeModel? Constructor, IEnumerable<ICodeModel> Properties, IEnumerable<ICodeModel> Functions) : ICodeModel {

    private string ConstructorAsString(int indent) => Constructor is null ? "" : $"{Indent(indent + 1)}public {Type}{Constructor.ToString(indent + 1)}";

    private string Abstract => Constructor is null ? "abstract " : "";

    public string ToString(int indent) =>
        $@"{Indent(indent)}public {Abstract}class {Type} {{
{ConstructorAsString(indent)}
{Properties.AsLineSeparatedString(indent + 1)}
{Functions.AsLineSeparatedString(indent + 1)}
{Indent(indent)}}}";

    public override string ToString() => ToString(0);
}