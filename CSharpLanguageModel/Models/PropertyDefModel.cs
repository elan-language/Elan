namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record PropertyDefModel(ICodeModel Id, ICodeModel Type) : ICodeModel {
    public string ToString(int indent) => $@"{Indent(indent)}public {Type} {Id} {{ get; set; }}";

    public override string ToString() => ToString(0);
}