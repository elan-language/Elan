namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record IfStatementModel(ICodeModel[] Expressions, ICodeModel[] StatementBlocks) : ICodeModel {
    public string ToString(int indent) {
        // check data 
        if (Expressions.Count() == StatementBlocks.Count() || StatementBlocks.Count() == Expressions.Count() + 1) {
            return $"{If(indent)}{ElseIfs(indent)}{Else(indent)}";
        }

        throw new CodeGenerationException("Mismatched count if expressions/blocks in If");
    }
    // expressions should always be equal to or one less than statemenblocks 

    private string If(int indent) =>
        $@"{Indent(indent)}if ({Expressions.First()}) {{
{StatementBlocks.First().ToString(indent + 1)}
{Indent(indent)}}}";

    private string Else(int indent) {
        return Expressions.Count() < StatementBlocks.Count() ? $@"
{Indent(indent)}else {{
{StatementBlocks.Last().ToString(indent + 1)}
{Indent(indent)}}}" : "";
    }

    private static string ElseIf(ICodeModel expression, ICodeModel statementBlock, int indent) =>
        $@"{Indent(indent)}else if ({expression}) {{
{statementBlock.ToString(indent + 1)}
{Indent(indent)}}}";

    private string ElseIfs(int indent) {
        if (Expressions.Count() > 1) {
            var elseIfExpressions = Expressions.Skip(1);
            var elseIfStatements = Expressions.Count() == StatementBlocks.Count() ? StatementBlocks.Skip(1) : StatementBlocks.Skip(1).SkipLast(1);
            var elseIfs = elseIfExpressions.Zip(elseIfStatements);

            var elseIfString = string.Join("\r\n", elseIfs.Select(ei => $"{ElseIf(ei.First, ei.Second, indent)}"));

            return $@"
{elseIfString}";
        }

        return "";
    }
}