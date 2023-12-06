namespace SymbolTable.SymbolTypes;

public record LambdaSymbolType(ISymbolType[] Arguments, ISymbolType ReturnType) : ISymbolType { }