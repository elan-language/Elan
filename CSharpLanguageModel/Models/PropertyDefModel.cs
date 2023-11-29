namespace CSharpLanguageModel.Models;

using static CodeHelpers;

[Flags]
public enum PropertyType {
    Abstract,
    Default,
    AbstractDefault,
    Immutable,
    Mutable
}

public record PropertyDefModel(ICodeModel Id, ICodeModel Type, PropertyType PropertyType, bool IsPrivate) : ICodeModel {
    private string Init =>
        PropertyType == PropertyType.Abstract
            ? ""
            : Type is IHasDefaultValue dm
                ? dm.DefaultValue
                : throw new NotImplementedException();

    private string Modifier => PropertyType switch {
        PropertyType.Abstract => "",
        PropertyType.AbstractDefault => "",
        PropertyType.Default => "override ",
        _ => "virtual "
    };

    private string Access => PropertyType switch {
        _ when IsPrivate => "protected",
        _ => "public"
    };

    private string Setter => PropertyType switch {
        PropertyType.Abstract => "",
        PropertyType.AbstractDefault => "",
        PropertyType.Immutable => " init;",
        _ => " set;"
    };

    private string Body => $"{{ get;{Setter} }}";

    private string Property =>
        PropertyType switch {
            PropertyType.Abstract => Body,
            PropertyType.Default => $"=> {Init}",
            PropertyType.AbstractDefault => $"=> {Init}",
            _ => $"{Body} = {Init}"
        };

    public string ToString(int indent) => $"{Indent(indent)}{Access} {Modifier}{Type} {Id} {Property}";
}