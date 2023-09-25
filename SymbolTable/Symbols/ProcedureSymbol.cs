using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolTable.Symbols
{
    public class ProcedureSymbol : MethodSymbol
    {
        public ProcedureSymbol(string name, IScope enclosingScope) : base(name, enclosingScope) { }

        public ProcedureSymbol(string name) : base(name) { }

        public override string ScopeName => "Procedure";
    }
}
