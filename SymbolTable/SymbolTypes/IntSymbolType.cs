using System.Data;

namespace SymbolTable.SymbolTypes;

public class IntSymbolType : ISymbolType {
    private IntSymbolType() { }

    public const string Name = "Int";

    public static IntSymbolType Instance { get; } = new();
}