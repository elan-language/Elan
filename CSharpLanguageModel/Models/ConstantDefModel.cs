namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ConstantDefModel(ICodeModel Id, string Type, ICodeModel Expression) : ICodeModel {
    public string ToString(int indent) => $@"{Indent(indent)}public {Type} {Id} = {Expression};";
}