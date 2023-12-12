using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using static SymbolTable.CSharpImportHelpers;

namespace SymbolTable;

public class SymbolTableImpl {
    public SymbolTableImpl() {
        InitTypeSystem(GlobalScope);
    }

    public GlobalScope GlobalScope { get; } = new();

}