using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class ParameterSymbol : ISymbol, IHasReturnType {
    public ParameterSymbol(string name, ISymbolType returnType, bool byRef, IScope enclosingScope) {
        Name = name;
        ReturnType = returnType;
        ByRef = byRef;
        EnclosingScope = enclosingScope;
    }

    public bool ByRef { get; }
    public IScope EnclosingScope { get; }

    public ISymbolType ReturnType { get; set; }
    public string Name { get; }

    public IScope? Scope { get; set; } = null;

    public override string ToString() => $"Parameter {Name} {ReturnType} {EnclosingScope}";
}