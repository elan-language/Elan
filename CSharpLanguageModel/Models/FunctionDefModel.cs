using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record FunctionDefModel(ICodeModel Signature, ICodeModel Statements, ICodeModel Return, bool Standalone) : ICodeModel {
    public override string ToString() => ToString(0);

    private string Qual => Standalone ? "static " : ""; 

    public string ToString(int indent) =>
        $@"{Indent(indent)}public {Qual}{Signature} {{
{Indent(Statements, indent + 1)}
{Indent(indent + 1)}return {Return};
{Indent(indent)}}}";
}