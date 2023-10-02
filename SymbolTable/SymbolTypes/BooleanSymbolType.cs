namespace SymbolTable.SymbolTypes;

public class BooleanSymbolType : ISymbolType {
    private BooleanSymbolType() { }

    public static BooleanSymbolType Instance { get; } = new();
}