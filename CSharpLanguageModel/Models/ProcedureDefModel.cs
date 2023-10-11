using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record ProcedureDefModel(ICodeModel Signature, ICodeModel Statements) : ICodeModel {
    public string ToString(int indent) => ToString();

    public override string ToString() =>
        $@"{Indent1}{Signature} {{
{Indent(Statements, 2)}
{Indent1}}}";
}