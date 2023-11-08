using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class ProcedureSymbol : MethodSymbol {
    public ProcedureSymbol(string name, IScope enclosingScope, NameSpace nameSpace) : base(name, VoidSymbolType.Instance, enclosingScope, nameSpace) { }

    public ProcedureSymbol(string name, NameSpace nameSpace) : base(name, VoidSymbolType.Instance, nameSpace) { }

    public override string ScopeName => $"Procedure: '{Name}'";
}