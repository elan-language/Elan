using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class VariableSymbol : ISymbol, IHasReturnType {
    public VariableSymbol(string name, ISymbolType returnType, IScope? enclosingScope) {
        Name = name;
        ReturnType = returnType;
        EnclosingScope = enclosingScope;
    }

    public IScope? EnclosingScope { get; }

    public ISymbolType ReturnType { get; set; }
    public string Name { get; }
    public IScope? Scope { get; set; } = null;

    public override string ToString() => $"Variable {Name} {ReturnType} {EnclosingScope}";
}