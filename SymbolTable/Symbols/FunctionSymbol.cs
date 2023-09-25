using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolTable.Symbols
{
    public class FunctionSymbol : MethodSymbol
    {
        public FunctionSymbol(string name, IScope enclosingScope) : base(name, enclosingScope) { }

        public FunctionSymbol(string name) : base(name) { }

        public override string ScopeName => "Function";
    }
}
