using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class VariableSymbol : ISymbol, IHasReturnType {
    public VariableSymbol(string name, ISymbolType returnType, IScope? enclosingScope) {
        Name = name;
        ReturnType = returnType;
        EnclosingScope = enclosingScope;
    }

    public ISymbolType ReturnType { get; set; }
    public IScope? EnclosingScope { get; }
    public string Name { get; }

    public IScope? Scope { get; set; } = null;
}