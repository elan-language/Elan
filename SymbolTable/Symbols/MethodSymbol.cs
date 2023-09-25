using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public abstract class MethodSymbol : BaseScope, ISymbol {
    protected MethodSymbol(string name, ISymbolType returnType, IScope enclosingScope) : this(name, returnType) => EnclosingScope = enclosingScope;

    protected MethodSymbol(string name, ISymbolType returnType) {
        Name = name;
        ReturnType = returnType;
    }

    public ISymbolType ReturnType { get; }
    public override IScope? EnclosingScope { get; }

    public string Name { get; }
    public IScope? Scope { get; set; }
}