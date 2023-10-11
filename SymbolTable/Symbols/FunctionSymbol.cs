using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class FunctionSymbol : MethodSymbol {
    public FunctionSymbol(string name, ISymbolType returnType, IScope enclosingScope) : base(name, returnType, enclosingScope) { }

    public FunctionSymbol(string name, ISymbolType returnType) : base(name, returnType) { }

    public override string ScopeName => "Function";
}