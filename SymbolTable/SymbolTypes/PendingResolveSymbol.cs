namespace SymbolTable.SymbolTypes;

public record PendingResolveSymbol : IPendingResolveSymbolType {
    public PendingResolveSymbol(string name) => Name = name;

    public string Name { get; }
}