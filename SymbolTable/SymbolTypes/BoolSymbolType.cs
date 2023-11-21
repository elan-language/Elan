namespace SymbolTable.SymbolTypes;

public class BoolSymbolType : ISymbolType {
    public const string Name = "Bool";
    private BoolSymbolType() { }
    public static BoolSymbolType Instance { get; } = new();
}