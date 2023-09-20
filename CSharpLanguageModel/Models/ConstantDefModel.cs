namespace CSharpLanguageModel.Models;

public record ConstantDefModel(ICodeModel Id, string Type, ICodeModel Expression) : ICodeModel {

    public string ToString(int indent) => ToString();
    public override string ToString() =>
        $@"
const {Type} {Id} = {Expression};
".Trim();
}