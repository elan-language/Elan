using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class GenericFunctionSymbol : FunctionSymbol {
    public GenericFunctionSymbol(string name, ISymbolType returnType, NameSpace nameSpace, string[] parameterNames, IDictionary<string, (int, int)> genericParameters, IScope? enclosingScope) : base(name, returnType, nameSpace, parameterNames, enclosingScope) => GenericParameters = genericParameters;

    public IDictionary<string, (int, int)> GenericParameters { get; }

    protected override string ScopeName => $"Generic Function '{Name}'";
}