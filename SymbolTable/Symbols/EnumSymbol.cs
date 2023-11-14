namespace SymbolTable.Symbols;

public class EnumSymbol : BaseScope, ISymbol {
    public EnumSymbol(string name, IScope enclosingScope) : this(name) => EnclosingScope = enclosingScope;

    public EnumSymbol(string name) => Name = name;

    public override IScope? EnclosingScope { get; }

    public override string ScopeName => $"Class '{Name}'";

    public string Name { get; }
    public IScope? Scope { get; set; }
}