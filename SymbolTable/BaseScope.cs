using SymbolTable.Symbols;

namespace SymbolTable;

public abstract class BaseScope : IScope {
    private readonly Dictionary<string, ISymbol> symbols = new();

    public abstract string ScopeName { get; }
    public IEnumerable<IScope> ChildScopes => Symbols.OfType<IScope>();
    public IEnumerable<ISymbol> Symbols => symbols.Values;
    public abstract IScope? EnclosingScope { get; }

    public virtual void Define(ISymbol symbol) {
        symbols[symbol.Name] = symbol;
        symbol.Scope = this;
    }

    public virtual ISymbol? Resolve(string name) => symbols.ContainsKey(name) ? symbols[name] : EnclosingScope?.Resolve(name);

    public override string ToString() => string.Join(", ", symbols.Keys);
}