using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record PropertyModel(ICodeModel Expression, ICodeModel Property) : ICodeModel {
    public override string ToString() => ToString(0);

    public string ToString(int indent) => $@"{Expression}.{Property}";
}