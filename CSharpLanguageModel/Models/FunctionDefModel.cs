using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;


public record FunctionDefModel(ICodeModel Signature, ICodeModel Statements, ICodeModel Return) : ICodeModel {

    public string ToString(int indent) => ToString();

    public override string ToString() =>
        $@"{Indent1}{Signature} {{
{Indent(Statements, 2)}
{Indent(2)}return {Return};
{Indent1}}}";
}