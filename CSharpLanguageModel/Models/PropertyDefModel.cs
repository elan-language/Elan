namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record PropertyDefModel(ICodeModel Id, ICodeModel Type, bool IsPrivate) : ICodeModel {

    private string Init => Type.ToString() == "string" ? @" = """";" : "";

    private string Access => IsPrivate ? @"private" : "public";

    public string ToString(int indent) => $@"{Indent(indent)}{Access} {Type} {Id} {{ get; set; }}{Init}";

    public override string ToString() => ToString(0);
}