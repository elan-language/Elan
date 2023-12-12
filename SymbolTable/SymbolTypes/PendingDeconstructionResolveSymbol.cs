namespace SymbolTable.SymbolTypes;

public record PendingDeconstructionResolveSymbol : IPendingResolveSymbolType {
    public PendingDeconstructionResolveSymbol(ISymbolType tuple, int index) {
        Index = index;
        Tuple = tuple;
    }

    public ISymbolType Tuple { get; }
    public int Index { get; }
}