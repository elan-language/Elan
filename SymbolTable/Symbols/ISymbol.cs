namespace SymbolTable.Symbols;

public interface ISymbol {
    public string Name { get; }

    public IScope? Scope { get; set; }
}