namespace CSharpLanguageModel.Models;

public record StatementBlockModel(ICodeModel[] Statements) : ICodeModel {
    public string ToString(int indent) => $"{Statements.AsLineSeparatedString(indent)}";
    public override string ToString() => ToString(0);
}