namespace SymbolTable.Symbols;

public class ScopedStatementSymbol : BaseScope, ISymbol {
    public ScopedStatementSymbol(string name, string[] parameterNames, IScope? enclosingScope) {
        Name = name;
        ParameterNames = parameterNames;
        EnclosingScope = enclosingScope;
    }

    public string[] ParameterNames { get; set; }
    public override IScope? EnclosingScope { get; }

    protected override string ScopeName => $"Scoped Statement '{Name}'";

    public string Name { get; }
    public IScope? Scope { get; set; }
}