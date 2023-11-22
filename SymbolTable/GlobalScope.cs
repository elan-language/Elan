namespace SymbolTable;

public class GlobalScope : BaseScope {
    protected override string ScopeName => "GlobalScope";
    public override IScope? EnclosingScope => null;
}