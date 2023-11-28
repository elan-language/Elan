using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record LambdaDefModel(IEnumerable<ICodeModel> Arguments, ICodeModel Expression) : ICodeModel {
    public string ToString(int indent) => $@"({Arguments.AsCommaSeparatedString()}) => {Expression}";

    public override string ToString() => ToString(0);
}