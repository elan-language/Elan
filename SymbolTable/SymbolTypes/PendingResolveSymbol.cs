namespace SymbolTable.SymbolTypes;

public class PendingResolveSymbol : ISymbolType {
    public PendingResolveSymbol(string name) => Name = name;

    public string Name { get; }
}