namespace SymbolTable.SymbolTypes;

public class VoidSymbolType : ISymbolType {
    private VoidSymbolType() { }

    public static VoidSymbolType Instance { get; } = new();
}