namespace SymbolTable.SymbolTypes;

public record PendingResolveSymbolType : IPendingResolveSymbolType {
    public PendingResolveSymbolType(string name) => Name = name;

    public string Name { get; }
}