namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record PropertyDefModel(ICodeModel Id, ICodeModel Type) : ICodeModel {

    private string Init => Type.ToString() == "string" ? @" = """";" : "";


    public string ToString(int indent) => $@"{Indent(indent)}public {Type} {Id} {{ get; set; }}{Init}";

    public override string ToString() => ToString(0);
}