namespace SymbolTable.SymbolTypes;

public class FloatSymbolType : ISymbolType {
    private FloatSymbolType() { }

    public const string Name = "Float";

    public static FloatSymbolType Instance { get; } = new();
}