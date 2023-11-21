namespace SymbolTable.SymbolTypes;

public class CharSymbolType : ISymbolType {
    public const string Name = "Char";
    private CharSymbolType() { }

    public static CharSymbolType Instance { get; } = new();
}