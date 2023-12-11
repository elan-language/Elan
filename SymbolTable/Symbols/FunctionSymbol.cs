using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class FunctionSymbol : MethodSymbol, IHasReturnType {
    public FunctionSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames, IScope? enclosingScope) : base(name, nameSpace, parameterNames, enclosingScope) => ReturnType = returnType;

    protected override string ScopeName => $"Function '{Name}'";
    public ISymbolType ReturnType { get; set; }
}