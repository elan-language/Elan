using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolTable.Symbols
{
    public interface ISymbol
    {
        public string Name { get; }

        public IScope? Scope { get; set; }
    }
}
