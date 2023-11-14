namespace SymbolTable.SymbolTypes;

public class EnumSymbolType : ISymbolType {
    public EnumSymbolType(string name) => Name = name;

    public string Name { get; }
}