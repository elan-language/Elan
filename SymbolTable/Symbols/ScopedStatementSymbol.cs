using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class ScopedStatementSymbol : BaseScope, ISymbol {
    public string[] ParameterNames { get; }
    public override  IScope? EnclosingScope { get; }

    public ScopedStatementSymbol(string name, string[] parameterNames, IScope? enclosingScope) {
        Name = name;
        ParameterNames = parameterNames;
        EnclosingScope = enclosingScope;
    }

    public string Name { get; }
    public IScope? Scope { get; set; }

    protected override string ScopeName => $"Function '{Name}'";
}