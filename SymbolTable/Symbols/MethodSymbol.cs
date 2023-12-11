using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public enum NameSpace {
    System,
    LibraryFunction,
    LibraryProcedure,
    UserGlobal,
    UserLocal
}

public abstract class MethodSymbol : BaseScope, ISymbol {
    protected MethodSymbol(string name, NameSpace nameSpace, string[] parameterNames, IScope? enclosingScope) {
        NameSpace = nameSpace;
        ParameterNames = parameterNames;
        Name = name;
        EnclosingScope = enclosingScope;
    }

    public NameSpace NameSpace { get; }
    public string[] ParameterNames { get; }

    public override IScope? EnclosingScope { get; }

    public string Name { get; }
    public IScope? Scope { get; set; }
}