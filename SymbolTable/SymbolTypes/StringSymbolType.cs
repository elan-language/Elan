namespace SymbolTable.SymbolTypes;

public record StringSymbolType : ISymbolType {
    public const string Name = "String";
    private StringSymbolType() { }
    public static StringSymbolType Instance { get; } = new();
}