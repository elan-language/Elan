namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record PropertyDefModel(ICodeModel Id, ICodeModel Type, bool IsPrivate, bool IsAbstract, bool IsDefault) : ICodeModel {
    private string Init =>
        IsAbstract
            ? ""
            : Type switch {
                ScalarValueModel { Value: "string" } => @""""";",
                ScalarValueModel => "default;",
                TypeModel => $"{Type}.DefaultInstance;",
                DataStructureTypeModel => $"{Type}.DefaultInstance;",
                _ => throw new NotImplementedException()
            };

    private string Modifier => IsDefault ? "override " : IsAbstract ? "" : "virtual ";

    private string Access => IsPrivate ? @"protected" : "public";

    private string Setter => IsAbstract ? "" : IsPrivate ? " set;" : " private set;";

    private string Body => $"{{ get;{Setter} }}";

    private string Property =>
        IsDefault
            ? $"=> {Init}"
            : IsAbstract
                ? Body
                : $"{Body} = {Init}";

    public string ToString(int indent) => $@"{Indent(indent)}{Access} {Modifier}{Type} {Id} {Property}";

    public override string ToString() => ToString(0);
}