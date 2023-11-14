using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class SystemCallSymbol : MethodSymbol {
    public SystemCallSymbol(string name, ISymbolType returnType, IScope currentScope,  NameSpace nameSpace) : base(name, returnType, currentScope, nameSpace) { }

    public SystemCallSymbol(string name, ISymbolType returnType, NameSpace nameSpace) : base(name, returnType, nameSpace) { }

    public override string ScopeName => $"SystemCall '{Name}'";
}