using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class SystemCallSymbol : MethodSymbol {
    public SystemCallSymbol(string name, ISymbolType returnType) : base(name, returnType, NameSpace.System) { }

    public override string ScopeName => $"SystemCall '{Name}'";
}