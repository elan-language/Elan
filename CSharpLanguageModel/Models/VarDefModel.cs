namespace CSharpLanguageModel.Models;

public record VarDefModel(ICodeModel Id, ICodeModel Expression) : ICodeModel {

    public override string ToString() => ToString(0);
    public  string ToString(int indent) =>
        $@"
var {Id} = {Expression};
".Trim();
}