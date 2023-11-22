namespace SymbolTable.SymbolTypes;

public record VoidSymbolType : ISymbolType {
    private VoidSymbolType() { }

    public static VoidSymbolType Instance { get; } = new();
}