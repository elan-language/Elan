namespace SymbolTable.SymbolTypes;

public record FloatSymbolType : ISymbolType {
    public const string Name = "Float";
    private FloatSymbolType() { }

    public static FloatSymbolType Instance { get; } = new();
}