namespace CSharpLanguageModel.Models;

public record DeconstructionModel(ICodeModel[] Variables, IEnumerable<bool> IsNew) : ICodeModel {
    private string VariableStr => string.Join(", ", Variables.Select((v, i) => $"{Prefix(i)}{v}"));

    public string ToString(int indent) => $"({VariableStr})";

    private string Prefix(int i) => IsNew.ToList()[i] ? "var " : "";

    public override string ToString() => ToString(0);
}