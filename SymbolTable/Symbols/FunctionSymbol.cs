using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class FunctionSymbol : MethodSymbol {
    public FunctionSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames, IScope? enclosingScope) : base(name, returnType, nameSpace, parameterNames, enclosingScope) { }

    protected override string ScopeName => $"Function '{Name}'";
}