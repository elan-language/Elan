namespace SymbolTable.SymbolTypes;

public record BoolSymbolType : ISymbolType {
    public const string Name = "Bool";
    private BoolSymbolType() { }
    public static BoolSymbolType Instance { get; } = new();
}