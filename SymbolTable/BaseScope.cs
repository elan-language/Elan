﻿using System.Data;
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

    public virtual void Define(ISymbol symbol) {
        if (symbols.ContainsKey(symbol.Name)) {
            throw new SymbolException($"Duplicate id '{symbol.Name}' in scope {ScopeName}");
        }

        symbols[symbol.Name] = symbol;
        symbol.Scope = this;
    }

    public virtual ISymbol? Resolve(string name) => symbols.TryGetValue(name, out var symbol) ? symbol : EnclosingScope?.Resolve(name);

    public override string ToString() => string.Join(", ", symbols.Keys);
}