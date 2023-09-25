using SymbolTable.Symbols;

namespace SymbolTable;

public interface IScope {
    ISymbol? Resolve(string name);
    IScope? EnclosingScope { get;  }
    void Define(ISymbol symbol);
}