using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record ProcedureDefModel(ICodeModel Signature, ICodeModel Statements, bool Standalone) : ICodeModel {
    public string ToString(int indent) => ToString();

    private string Qual => Standalone ? "static " : ""; 

    public override string ToString() =>
        $@"{Indent1}public {Qual}{Signature} {{
{Indent(Statements, 2)}
{Indent1}}}";
}