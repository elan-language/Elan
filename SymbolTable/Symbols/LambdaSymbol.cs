using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class LambdaSymbol : MethodSymbol, IHasReturnType {
    public LambdaSymbol(string name, ISymbolType returnType, string[] parameterNames, IScope? enclosingScope) : base(name, NameSpace.UserLocal, parameterNames, enclosingScope) => ReturnType = returnType;

    protected override string ScopeName => $"Function '{Name}'";
    public ISymbolType ReturnType { get; set; }
}