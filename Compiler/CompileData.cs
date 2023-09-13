using AbstractSyntaxTree.Nodes;

namespace Compiler;

public record CompileData {
    public string FileName { get; init; } = "";

    public string ElanCode { get; init; } = "";

    public ElanParser? Parser { get; init; }

    public ElanParser.FileContext? ParseTree { get; init; }

    public IAstNode? AbstractSyntaxTree { get; init; }

    public string ObjectCode { get; init; } = "";

    public string ObjectCodeCompileStdOut { get; init; } = "";

    public string ObjectCodeCompileStdErr { get; init; } = "";
}