namespace SymbolTable.SymbolTypes;

public class CharSymbolType : ISymbolType {
    private CharSymbolType() { }

    public const string Name = "Char";

    public static CharSymbolType Instance { get; } = new();
}