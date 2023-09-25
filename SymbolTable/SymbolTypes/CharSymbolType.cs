namespace SymbolTable.SymbolTypes;

public class CharSymbolType : ISymbolType {
    private CharSymbolType() { }

    public static CharSymbolType Instance { get; } = new();
}