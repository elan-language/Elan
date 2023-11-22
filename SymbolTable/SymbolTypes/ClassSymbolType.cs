namespace SymbolTable.SymbolTypes;

public record ClassSymbolType : ISymbolType {
    public ClassSymbolType(string name) => Name = name;

    public string Name { get; }
}