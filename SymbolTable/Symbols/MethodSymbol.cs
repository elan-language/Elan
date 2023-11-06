using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public enum NameSpace {
    System,
    Library,
    UserGlobal, 
    UserLocal
}

public abstract class MethodSymbol : BaseScope, ISymbol {
    protected MethodSymbol(string name, ISymbolType returnType, IScope enclosingScope, NameSpace nameSpace) : this(name, returnType, nameSpace) => EnclosingScope = enclosingScope;

    protected MethodSymbol(string name, ISymbolType returnType, NameSpace nameSpace) {
        NameSpace = nameSpace;
        Name = name;
        ReturnType = returnType;
    }

    public NameSpace NameSpace { get; }

    public ISymbolType ReturnType { get; }
    public override IScope? EnclosingScope { get; }

    public string Name { get; }
    public IScope? Scope { get; set; }
}