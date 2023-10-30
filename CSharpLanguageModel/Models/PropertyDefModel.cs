namespace CSharpLanguageModel.Models;

using static CodeHelpers;

[Flags]
public enum PropertyType {
    Abstract,
    Default,
    Immutable,
    Mutable
}

public record PropertyDefModel(ICodeModel Id, ICodeModel Type, PropertyType PropertyType, bool IsPrivate) : ICodeModel {
    private string Init =>
        PropertyType == PropertyType.Abstract
            ? ""
            : Type switch {
                ScalarValueModel { Value: "string" } => @""""";",
                ScalarValueModel => "default;",
                TypeModel => $"{Type}.DefaultInstance;",
                DataStructureTypeModel => $"{Type}.DefaultInstance;",
                _ => throw new NotImplementedException()
            };

    private string Modifier => PropertyType switch {
        PropertyType.Abstract => "",
        PropertyType.Default => "override ",
        _ => "virtual "
    };

    private string Access => PropertyType switch {
        _ when IsPrivate => @"protected",
        _ => "public"
    };

    private string Setter => PropertyType switch {
        PropertyType.Abstract => "",
        PropertyType.Immutable => " init;",
        _ when IsPrivate  => " set;",
        _ => " private set;"
    };

    private string Body => $"{{ get;{Setter} }}";

    private string Property =>
        PropertyType switch {
            PropertyType.Abstract => Body,
            PropertyType.Default => $"=> {Init}",
            _ => $"{Body} = {Init}"
        };

    public string ToString(int indent) => $@"{Indent(indent)}{Access} {Modifier}{Type} {Id} {Property}";

    public override string ToString() => ToString(0);
}