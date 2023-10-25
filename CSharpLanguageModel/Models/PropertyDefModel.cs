namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record PropertyDefModel(ICodeModel Id, ICodeModel Type, bool IsPrivate, bool IsAbstract) : ICodeModel {

    private string Init => Type.ToString() == "string" ? @" = """";" : "";

    private string Access => IsPrivate ? @"private" : "public";

    private string Setter => IsAbstract ? "" : IsPrivate ? " set;" : " private set;";

    public string ToString(int indent) => $@"{Indent(indent)}{Access} {Type} {Id} {{ get;{Setter} }}{Init}";

    public override string ToString() => ToString(0);
}