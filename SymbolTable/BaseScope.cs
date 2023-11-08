using System.Data;
using SymbolTable.Symbols;

namespace SymbolTable;

public abstract class BaseScope : IScope {
    private readonly Dictionary<string, ISymbol> symbols = new();

    public abstract string ScopeName { get; }
    public IEnumerable<IScope> ChildScopes => Symbols.OfType<IScope>();
    public IEnumerable<ISymbol> Symbols => symbols.Values;
    public abstract IScope? EnclosingScope { get; }

    public virtual void DefineSystem(ISymbol symbol) {
        symbols[symbol.Name] = symbol;
        symbol.Scope = this;
    }

    private static string NormalizeName(string name) => name.Length > 1 ? $"{name[0]}{name[1..].ToLower()}" : name; 

    public virtual void Define(ISymbol symbol) {
        if (symbols.Keys.Select(NormalizeName).Contains(NormalizeName(symbol.Name))) {
            throw new SymbolException($"Duplicate id '{symbol.Name}' in scope {ScopeName}");
        }

        symbols[symbol.Name] = symbol;
        symbol.Scope = this;
    }

    public virtual ISymbol? Resolve(string name) => symbols.TryGetValue(name, out var symbol) ? symbol : EnclosingScope?.Resolve(name);

    public override string ToString() => string.Join(", ", symbols.Keys);
}