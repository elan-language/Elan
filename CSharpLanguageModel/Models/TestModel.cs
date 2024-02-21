using static CSharpLanguageModel.CodeHelpers;

namespace CSharpLanguageModel.Models;

public record TestModel(ICodeModel Id, ICodeModel Statements) : ICodeModel {
    public string ToString(int indent) =>
        $@"{Indent(indent)}[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
{Indent(indent)}public void {Id}() {{
{Indent(Statements, indent + 1)}
{Indent(indent)}}}";
}