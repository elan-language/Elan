namespace CSharpLanguageModel.Models;

public record ConstantDefModel(ICodeModel Id, string Type, ICodeModel Expression) : ICodeModel {
    public override string ToString() =>
        $@"
const {Type} {Id} = {Expression};
".Trim();
}