namespace SymbolTable.SymbolTypes;

public record PendingTupleResolveSymbol : IPendingResolveSymbolType {
    public PendingTupleResolveSymbol(ISymbolType tuple, int index) {
        Index = index;
        Tuple = tuple;
    }

    public ISymbolType Tuple { get; }
    public int Index { get; }
}