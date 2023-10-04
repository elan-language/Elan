using System.Linq.Expressions;

namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ParameterModel(ICodeModel Id, ICodeModel Type) : ICodeModel {

    public override string ToString() => ToString(0);
    public  string ToString(int indent) => $@"{Type} {Id}";
}