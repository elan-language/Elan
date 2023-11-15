using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class SystemAccessorSymbol : MethodSymbol {
    public SystemAccessorSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames, IScope currentScope) : base(name, returnType, nameSpace, parameterNames, currentScope) { }

    public SystemAccessorSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames) : base(name, returnType, nameSpace, parameterNames) { }

    public override string ScopeName => $"SystemCall '{Name}'";
}