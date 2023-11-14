using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class SystemAccessorSymbol : MethodSymbol {
    public SystemAccessorSymbol(string name, ISymbolType returnType, IScope currentScope,  NameSpace nameSpace) : base(name, returnType, currentScope, nameSpace) { }

    public SystemAccessorSymbol(string name, ISymbolType returnType, NameSpace nameSpace) : base(name, returnType, nameSpace) { }

    public override string ScopeName => $"SystemCall '{Name}'";
}