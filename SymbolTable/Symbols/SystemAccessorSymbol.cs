using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class SystemAccessorSymbol : MethodSymbol, IHasReturnType {
    public SystemAccessorSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames, IScope? currentScope) : base(name, nameSpace, parameterNames, currentScope) => ReturnType = returnType;

    protected override string ScopeName => $"SystemCall '{Name}'";
    public ISymbolType ReturnType { get; set; }
}