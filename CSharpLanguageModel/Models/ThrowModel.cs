namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ThrowModel(ICodeModel Thrown) : ICodeModel {
    public string ToString(int indent) => $"{Indent(indent)}throw new StandardLibrary.ElanException({Thrown});";
}