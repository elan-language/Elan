namespace SymbolTable.SymbolTypes;

public record IntSymbolType : ISymbolType {
    public const string Name = "Int";
    private IntSymbolType() { }

    public static IntSymbolType Instance { get; } = new();
}