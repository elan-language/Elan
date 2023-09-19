using AbstractSyntaxTree;
using Antlr4.Runtime;
using CSharpLanguageModel;

namespace Compiler;

public static class Pipeline {
    public static CompileData Compile(string fileName) {
        var compileData = new CompileData { FileName = fileName };
        compileData = ReadCode(compileData);
        return Compile(compileData);
    }

    public static CompileData Compile(CompileData compileData) {
        compileData = ParseCode(compileData);
        compileData = CompileCode(compileData);
        compileData = GenerateObjectCode(compileData);
        compileData = CompileObjectCode(compileData);

        return compileData;
    }

    private static CompileData ReadCode(CompileData compileData) {
        var code = File.ReadAllText(compileData.FileName);
        return compileData with { ElanCode = code };
    }

    private static CompileData ParseCode(CompileData compileData) {
        if (string.IsNullOrWhiteSpace(compileData.ElanCode)) {
            return compileData;
        }

        compileData = NormalizeCode(compileData);

        var parser = GetParser(compileData.ElanCode);
        var parseTree = parser.file();
        var parseStringTree = parseTree.ToStringTree(parser);

        var syntaxErrors = parser.ErrorListeners.OfType<ErrorListener>().First().SyntaxErrors.ToArray();

        return compileData with { ParserErrors = syntaxErrors, ParseTree = parseTree, ParseStringTree = parseStringTree };
    }

    private static CompileData NormalizeCode(CompileData compileData) {
        var code = compileData.ElanCode;

        if (code.StartsWith(ElanSymbols.Comment)) {
            return compileData;
        }

        code = $"{ElanSymbols.Comment}{ElanSymbols.NewLine}{code}";

        return compileData with { ElanCode = code };
    }

    private static CompileData CompileCode(CompileData compileData) {
        if (compileData.ParseTree is null || compileData.ParserErrors.Length > 0) {
            return compileData;
        }

        var visitor = new ParseTreeVisitor();
        var ast = visitor.Visit(compileData.ParseTree);

        return compileData with { AbstractSyntaxTree = ast };
    }

    private static ElanParser GetParser(string code) {
        var inputStream = new AntlrInputStream(code);
        var lexer = new ElanLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new ElanParser(tokenStream);

        parser.RemoveErrorListeners();
        parser.AddErrorListener(new ErrorListener());

        return parser;
    }

    private static CompileData GenerateObjectCode(CompileData compileData) {
        if (compileData.AbstractSyntaxTree is null) {
            return compileData;
        }

        var codeVisitor = new CodeModelAstVisitor();
        var codeModel = codeVisitor.Visit(compileData.AbstractSyntaxTree);
        var objectCode = codeModel.ToString() ?? "";

        return compileData with { ObjectCode = objectCode };
    }

    private static CompileData CompileObjectCode(CompileData compileData) {
        if (string.IsNullOrWhiteSpace(compileData.ObjectCode)) {
            return compileData;
        }

        var (fileName, stdOut, stdErr) = CSharpCompiler.CompileObjectCode(compileData.FileName, compileData.ObjectCode);

        return compileData with { FileName = fileName, ObjectCodeCompileStdOut = stdOut, ObjectCodeCompileStdErr = stdErr };
    }
}