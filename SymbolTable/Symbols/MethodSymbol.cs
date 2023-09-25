using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public abstract class MethodSymbol : BaseScope, ISymbol
{
    protected MethodSymbol(string name, ISymbolType returnType, IScope enclosingScope) : this(name, returnType) {
        EnclosingScope = enclosingScope;
    }

    protected MethodSymbol(string name, ISymbolType returnType) {
        Name = name;
        ReturnType = returnType;
    }

    public string Name { get; }

    public ISymbolType ReturnType { get; }
    public IScope? Scope { get; set; }
    public override IScope? EnclosingScope { get; }
}