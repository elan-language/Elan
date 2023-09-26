namespace CSharpLanguageModel.Models;

public record RangeExpressionModel : ICodeModel {
    private readonly ICodeModel from;
    private readonly string prefixStr;
    private readonly string suffixStr;
    private readonly string toStr;

    public RangeExpressionModel(bool prefix, ICodeModel from, ICodeModel? to) {
        prefixStr = prefix ? ".." : "";
        suffixStr = prefix ? "" : "..";
        this.from = from;
        toStr = to is null ? "" : $"({to})";
    }

    public string ToString(int indent) => $"{prefixStr}({from}){suffixStr}{toStr}";

    public override string ToString() => ToString(0);
}