using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols
{
    public class FunctionSymbol : MethodSymbol
    {
        public FunctionSymbol(string name, ISymbolType returnType,  IScope enclosingScope) : base(name, returnType, enclosingScope) { }

        public FunctionSymbol(string name, ISymbolType returnType) : base(name, returnType) { }

        public override string ScopeName => "Function";
    }
}
