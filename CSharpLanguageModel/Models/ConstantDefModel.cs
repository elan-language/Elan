namespace CSharpLanguageModel.Models;

public record ConstantDefModel(ICodeModel Id, string Type, ICodeModel Expression) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) =>
        $@"
const {Type} {Id} = {Expression};
".Trim();
}