namespace SymbolTable.SymbolTypes;

public record PendingResolveSymbol : ISymbolType {
    public PendingResolveSymbol(string name) => Name = name;

    public string Name { get; }
}