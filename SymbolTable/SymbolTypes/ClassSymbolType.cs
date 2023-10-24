namespace SymbolTable.SymbolTypes;

public class ClassSymbolType : ISymbolType {
    public ClassSymbolType(string name) => Name = name;

    public string Name { get; }
}