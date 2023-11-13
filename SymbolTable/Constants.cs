namespace SymbolTable;

public static class Constants {
    public const string WellKnownMainId = "_main";
    public const string WellKnownConstructorId = "_constructor";

    public static readonly string[] ElanKeywords = {
        "abstract", "case", "catch", "class",
        "constant", "constructor", "curry", "default", "else", "end",
        "enum", "for", "foreach", "function", "global", "if",
        "immutable", "in", "inherits", "lambda", "let", "main", "partial",
        "private", "procedure", "property", "repeat", "return", "self",
        "step", "switch", "test", "then", "to", "try", "until",
        "var", "while", "with", "Array", "List", "Dictionary",
        "Iter", "mod", "div",
        "and", "not", "or", "xor"
    };
}