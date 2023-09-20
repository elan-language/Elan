namespace CSharpLanguageModel.Models;

public record VarDefModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {

    public string ToString(int indent) => ToString();
    public override string ToString() =>
        $@"
var {Id} = {Expression};
".Trim();
}