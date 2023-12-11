using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class ParameterSymbol : ISymbol, IHasReturnType {
    public ParameterSymbol(string name, ISymbolType returnType, bool byRef, IScope enclosingScope) {
        Name = name;
        ReturnType = returnType;
        ByRef = byRef;
        EnclosingScope = enclosingScope;
    }

    public ISymbolType ReturnType { get; set;  }
    public bool ByRef { get; }
    public IScope EnclosingScope { get; }
    public string Name { get; }

    public IScope? Scope { get; set; } = null;
}