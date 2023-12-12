namespace CSharpLanguageModel.Models;

public record InputModel(string? Prompt) : ICodeModel {
    private string prompt => Prompt is null ? "" : $" System.Console.Write({Prompt}); ";

    public string ToString(int indent) => $@"((Func<string>)(() => {{{prompt}return Console.ReadLine() ?? """";}}))()";

    public override string ToString() => ToString(0);
}