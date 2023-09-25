﻿using SymbolTable.SymbolTypes;

namespace SymbolTable.Symbols;

public class SystemCallSymbol : MethodSymbol {
    public SystemCallSymbol(string name, ISymbolType returnType) : base(name, returnType) { }

    public override string ScopeName => "SystemCall";
}