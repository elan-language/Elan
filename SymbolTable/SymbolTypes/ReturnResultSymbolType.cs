namespace SymbolTable.SymbolTypes;

public class ReturnResultSymbolType : ISymbolType {
    public ReturnResultSymbolType(string name) => Name = name;

    public string Name { get; }
}