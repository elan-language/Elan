namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record VarDefModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {
    public string ToString(int indent) => $@"{Indent(indent)}var {Id} = {Expression};";
}