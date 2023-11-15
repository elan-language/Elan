using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public enum NameSpace {
    System,
    Library,
    UserGlobal,
    UserLocal
}

public abstract class MethodSymbol : BaseScope, ISymbol {
    protected MethodSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames, IScope enclosingScope) : this(name, returnType, nameSpace, parameterNames) => EnclosingScope = enclosingScope;

    protected MethodSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames) {
        NameSpace = nameSpace;
        ParameterNames = parameterNames;
        Name = name;
        ReturnType = returnType;
    }

    public NameSpace NameSpace { get; }
    public string[] ParameterNames { get; }

    public ISymbolType ReturnType { get; }
    public override IScope? EnclosingScope { get; }

    public string Name { get; }
    public IScope? Scope { get; set; }
}