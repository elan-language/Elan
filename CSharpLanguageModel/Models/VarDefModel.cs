namespace CSharpLanguageModel.Models;

public record VarDefModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {
    public override string ToString() =>
        $@"
var {Id} = {Expression};
".Trim();
}