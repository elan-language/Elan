using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class FunctionSymbol : MethodSymbol {
    public FunctionSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames, IScope enclosingScope) : base(name, returnType, nameSpace, parameterNames, enclosingScope) { }

    public FunctionSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames) : base(name, returnType, nameSpace, parameterNames) { }

    public override string ScopeName => $"Function '{Name}'";
}