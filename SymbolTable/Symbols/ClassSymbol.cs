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

    public ClassSymbol(string name) => Name = name;

    public ClassSymbolTypeType ClassType { get; }
    public override IScope? EnclosingScope { get; }

    public override string ScopeName => "Class";

    public string Name { get; }
    public IScope? Scope { get; set; }
}