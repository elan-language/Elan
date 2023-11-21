namespace SymbolTable.SymbolTypes;

public class IntSymbolType : ISymbolType {
    public const string Name = "Int";
    private IntSymbolType() { }

    public static IntSymbolType Instance { get; } = new();
}