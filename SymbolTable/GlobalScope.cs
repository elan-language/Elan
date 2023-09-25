namespace SymbolTable;

public class GlobalScope : BaseScope {
    public override string ScopeName => "GlobalScope";
    public override IScope? EnclosingScope => null;
}