namespace SymbolTable.SymbolTypes;

public class DecimalSymbolType : ISymbolType {
    private DecimalSymbolType() { }

    public static DecimalSymbolType Instance { get; } = new();
}