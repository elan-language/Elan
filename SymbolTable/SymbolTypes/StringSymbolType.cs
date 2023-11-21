namespace SymbolTable.SymbolTypes;

public class StringSymbolType : ISymbolType {
    public const string Name = "String";
    private StringSymbolType() { }
    public static StringSymbolType Instance { get; } = new();
}