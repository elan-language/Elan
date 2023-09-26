namespace CSharpLanguageModel.Models;

public record NewInstanceModel : ICodeModel {
    public string ToString(int indent) => @"";

    public override string ToString() => ToString(0);
}