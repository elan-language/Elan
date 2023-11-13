namespace SymbolTable.SymbolTypes;

public class BoolSymbolType : ISymbolType {
    private BoolSymbolType() { }

    public const string Name = "Bool";
    public static BoolSymbolType Instance { get; } = new();
}