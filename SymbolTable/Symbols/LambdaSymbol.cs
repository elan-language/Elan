using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class LambdaSymbol : MethodSymbol {
    public LambdaSymbol(string name, ISymbolType returnType, string[] parameterNames, IScope? enclosingScope) : base(name, returnType, NameSpace.UserLocal, parameterNames, enclosingScope) { }

    protected override string ScopeName => $"Function '{Name}'";
}