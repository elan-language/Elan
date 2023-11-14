namespace SymbolTable.SymbolTypes;

public class PendingTupleResolveSymbol : ISymbolType {
    public PendingTupleResolveSymbol(string name, int index) {
        Name = name;
        Index = index;
    }

    public string Name { get; }
    public int Index { get; }
}