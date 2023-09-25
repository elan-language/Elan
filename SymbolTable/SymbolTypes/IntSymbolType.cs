namespace SymbolTable.SymbolTypes;

public class IntSymbolType : ISymbolType {
    private IntSymbolType() { }

    public static IntSymbolType Instance { get; } = new();
}