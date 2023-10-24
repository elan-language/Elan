namespace SymbolTable.Symbols;

public class ClassSymbol : BaseScope, ISymbol {
    public ClassSymbol(string name, IScope enclosingScope) : this(name) => EnclosingScope = enclosingScope;

    public ClassSymbol(string name) => Name = name;

    public override IScope? EnclosingScope { get; }

    public string Name { get; }
    public IScope? Scope { get; set; }

    public override string ScopeName => "Class";
}