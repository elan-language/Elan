using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols; 

public class ProcedureSymbol : MethodSymbol {
    public ProcedureSymbol(string name, IScope enclosingScope) : base(name, VoidSymbolType.Instance, enclosingScope) { }

    public ProcedureSymbol(string name) : base(name, VoidSymbolType.Instance) { }

    public override string ScopeName => "Procedure";
}