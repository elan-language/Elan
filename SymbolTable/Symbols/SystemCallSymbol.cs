using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolTable.Symbols
{
    public class SystemCallSymbol : MethodSymbol
    {
        public SystemCallSymbol(string name, IScope enclosingScope) : base(name, enclosingScope) { }

        public SystemCallSymbol(string name) : base(name) { }

        public override string ScopeName => "SystemCall";
    }
}
