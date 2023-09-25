namespace SymbolTable.SymbolTypes;

public class FloatSymbolType : ISymbolType {
    private FloatSymbolType() { }

    public static FloatSymbolType Instance { get; } = new();
}