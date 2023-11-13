namespace SymbolTable.SymbolTypes;

public class StringSymbolType : ISymbolType {
    private StringSymbolType() { }


    public const string Name = "String";
    public static StringSymbolType Instance { get; } = new();
}