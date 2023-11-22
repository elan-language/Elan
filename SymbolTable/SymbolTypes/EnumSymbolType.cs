namespace SymbolTable.SymbolTypes;

public record EnumSymbolType : ISymbolType {
    public EnumSymbolType(string name) => Name = name;

    public string Name { get; }
}