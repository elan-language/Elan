namespace SymbolTable.SymbolTypes;

public class StringSymbolType : ISymbolType {
    private StringSymbolType() { }

    public static StringSymbolType Instance { get; } = new();
}