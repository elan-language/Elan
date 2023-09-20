namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record IfStatementModel(IEnumerable<ICodeModel> Expressions, IEnumerable<ICodeModel> StatementBlocks) : ICodeModel {
    // expressions should always be equal to or one less than statemenblocks 

    private string ElseIf(ICodeModel expression, ICodeModel statementBlock) =>
        $@"
else if ({expression}) then {{
{Indent1}{statementBlock}
}}
".Trim();

    private string ElseIfs() {
        if (Expressions.Count() > 1) {
            var elseIfExpressions = Expressions.Skip(1);
            var elseIfStatements = Expressions.Count() == StatementBlocks.Count() ? StatementBlocks.Skip(1) : StatementBlocks.Skip(1).SkipLast(1);
            var elseIfs = elseIfExpressions.Zip(elseIfStatements);

            return string.Join("\r\n", elseIfs.Select(ei => $"{ElseIf(ei.First, ei.Second)}")).TrimEnd();
        }

        return "";
    }

    private string Else() {
        if (Expressions.Count() < StatementBlocks.Count()) {
            return $@"
else {{
{Indent1}{StatementBlocks.Last()}
}}
".Trim();
        }

        return "";
    }

    public string ToString(int indent) {

        string If() => $@"
if ({Expressions.First()}) then {{
{Indent1}{StatementBlocks.First()}
}}".Trim();

        // check data 
        if (Expressions.Count() == StatementBlocks.Count() || StatementBlocks.Count() == Expressions.Count() + 1) {
            return $@"
{If()}
{ElseIfs()}
{Else()}
";
        }

        throw new CodeGenerationException("Mismatched count if expressions/blocks in If");
    }
}