namespace SymbolTable.SymbolTypes;

public record TupleSymbolType(ISymbolType[] Types) : ISymbolType { }