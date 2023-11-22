namespace SymbolTable.SymbolTypes;

public record CharSymbolType : ISymbolType {
    public const string Name = "Char";
    private CharSymbolType() { }

    public static CharSymbolType Instance { get; } = new();
}