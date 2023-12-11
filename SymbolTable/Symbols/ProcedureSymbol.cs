using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class ProcedureSymbol : MethodSymbol {
    public ProcedureSymbol(string name, NameSpace nameSpace, string[] parameterNames, IScope? enclosingScope) : base(name, nameSpace, parameterNames, enclosingScope) { }

    protected override string ScopeName => Name switch {
        Constants.WellKnownMainId => "'main'",
        Constants.WellKnownConstructorId => "'constructor'",
        _ => $"Procedure: '{Name}'"
    };
}