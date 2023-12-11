using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols; 

public interface IHasReturnType {
    public ISymbolType ReturnType { get; set; }
}