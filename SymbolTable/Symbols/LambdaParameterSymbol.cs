using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class LambdaParameterSymbol : ISymbol {
    public LambdaParameterSymbol(string name, ISymbolType returnType, IScope enclosingScope) {
        Name = name;
        ReturnType = returnType;

        EnclosingScope = enclosingScope;
    }

    public ISymbolType ReturnType { get; }

    public IScope EnclosingScope { get; }
    public string Name { get; }

    public IScope? Scope { get; set; } = null;
}