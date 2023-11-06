using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class FunctionSymbol : MethodSymbol {
    public FunctionSymbol(string name, ISymbolType returnType, IScope enclosingScope, NameSpace nameSpace) : base(name, returnType, enclosingScope, nameSpace) { }

    public FunctionSymbol(string name, ISymbolType returnType, NameSpace nameSpace) : base(name, returnType, nameSpace) { }

    public override string ScopeName => "Function";
}