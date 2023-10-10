namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record TryCatchModel(ICodeModel TriedCode, ICodeModel Id, ICodeModel CaughtCode) : ICodeModel {
    public string ToString(int indent) =>
        // check data 
        $@"{Indent(indent)}try {{
{Indent(TriedCode, indent + 1)}
{Indent(indent)}}}
{Indent(indent)}catch (Exception _{Id}) {{
{Indent(indent + 1)}var {Id} = new StandardLibrary.ElanException(_{Id});
{Indent(CaughtCode, indent + 1)}
{Indent(indent)}}}";
}