namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ConstantDefModel(ICodeModel Id, string Type, ICodeModel Expression) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) => $@"{Indent(indent)}public const {Type} {Id} = {Expression};";
}