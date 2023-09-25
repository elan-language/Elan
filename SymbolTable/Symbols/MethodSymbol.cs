namespace SymbolTable.Symbols;

public abstract class MethodSymbol : BaseScope, ISymbol
{
    protected MethodSymbol(string name, IScope enclosingScope) : this(name) {
        EnclosingScope = enclosingScope;
    }

    protected MethodSymbol(string name) {
        Name = name;
    }

    public string Name { get; }
    public IScope? Scope { get; set; }
    public override IScope? EnclosingScope { get; }
}