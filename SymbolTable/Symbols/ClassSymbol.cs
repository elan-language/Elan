namespace SymbolTable.Symbols;

public enum ClassSymbolTypeType {
    Mutable,
    Immutable,
    Abstract
}

public class ClassSymbol : BaseScope, ISymbol {
    public ClassSymbol(string name, ClassSymbolTypeType classType, IScope enclosingScope) : this(name) {
        ClassType = classType;
        EnclosingScope = enclosingScope;
    }

    private ClassSymbol(string name) => Name = name;

    public ClassSymbolTypeType ClassType { get; }
    public override IScope? EnclosingScope { get; }

    protected override string ScopeName => $"Class '{Name}'";

    public string Name { get; }
    public IScope? Scope { get; set; }
}