namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record PrintModel(ICodeModel? Expression) : ICodeModel {
    private string ExpressionStr => Expression is null ? "" : $"StandardLibrary.Functions.asString({Expression})";

    public string ToString(int indent) => $@"{Indent(indent)}System.Console.WriteLine({ExpressionStr});";
}