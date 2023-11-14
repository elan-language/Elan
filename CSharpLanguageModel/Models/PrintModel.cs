namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record PrintModel(ICodeModel Expression) : ICodeModel {
    public string ToString(int indent) => $@"{Indent(indent)}System.Console.WriteLine(StandardLibrary.Functions.asString({Expression}));";
    public override string ToString() => ToString(0);
}
