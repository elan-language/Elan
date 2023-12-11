using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using static SymbolTable.CSharpImportHelpers;

namespace SymbolTable;

public class SymbolTableImpl {
    public SymbolTableImpl() {
        InitTypeSystem(GlobalScope);
    }

    public GlobalScope GlobalScope { get; } = new();

    public void Validate() {
        ValidateScope(GlobalScope);
    }

    private void ValidateScope(IScope scope) {
        var symbols = ((BaseScope)scope).Symbols;

        foreach (var symbol in symbols) {
            if (symbol is IHasReturnType { ReturnType: IPendingResolveSymbolType }) {
                throw new NotImplementedException(symbol.ToString());
            }

            if (symbol is IScope subScope) {
                ValidateScope(subScope);
            }
        }
    }
}