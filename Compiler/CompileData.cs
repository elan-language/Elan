﻿using AbstractSyntaxTree.Nodes;
using SymbolTable;

namespace Compiler;

public record CompileData {
    public string FileName { get; init; } = "";

    public string ElanCode { get; init; } = "";

    public ParserErrorException[] ParserErrors { get; init; } = Array.Empty<ParserErrorException>();

    public string[] CompileErrors { get; init; } = { "Compile second pass not run" };

    public ElanParser.FileContext? ParseTree { get; init; }

    public string ParseStringTree { get; init; } = "";

    public IAstNode? AbstractSyntaxTree { get; init; }

    public SymbolTableImpl? SymbolTable { get; init; }

    public string ObjectCode { get; init; } = "";

    public string ObjectCodeCompileStdOut { get; init; } = "";

    public string ObjectCodeCompileStdErr { get; init; } = "";
}