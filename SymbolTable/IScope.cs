using SymbolTable.Symbols;

namespace SymbolTable;

public interface IScope {
    IScope? EnclosingScope { get; }
    ISymbol? Resolve(string name);
    void Define(ISymbol symbol);
}