﻿using System.Security.Cryptography;
using System.Text;
using AbstractSyntaxTree;
using Antlr4.Runtime;
using CSharpLanguageModel;
using SymbolTable;

namespace Compiler;

public static class Pipeline {
    public static CompileData Compile(string fileName, string workingDir) {
        var compileData = new CompileData { FileName = fileName, WorkingDirectory = workingDir };
        compileData = ReadCode(compileData);
        return Compile(compileData);
    }

    public static CompileData Compile(CompileData compileData) {
        compileData = ValidateHeader(compileData);
        compileData = ParseCode(compileData);
        compileData = CompileCode(compileData);
        compileData = GenerateSymbolTable(compileData);
        compileData = SecondPass(compileData);
        compileData = ThirdPass(compileData);
        compileData = GenerateObjectCode(compileData);
        compileData = CompileObjectCode(compileData);

        return compileData;
    }

    private static CompileData ReadCode(CompileData compileData) {
        var code = File.ReadAllText(compileData.FileName);
        return compileData with { ElanCode = code };
    }

    private static CompileData ParseCode(CompileData compileData) {
        if (compileData.HeaderErrors.Length > 0) {
            return compileData;
        }

        compileData = ValidateHeader(compileData);

        var parser = GetParser(compileData.ElanCode);
        var parseTree = parser.file();
        var parseStringTree = parseTree.ToStringTree(parser);

        var syntaxErrors = parser.ErrorListeners.OfType<ErrorListener>().First().SyntaxErrors.ToArray();

        return compileData with { ParserErrors = syntaxErrors, ParseTree = parseTree, ParseStringTree = parseStringTree };
    }

    private static CompileData ValidateHeader(CompileData compileData) {
        var code = compileData.ElanCode;
        var firstLine = code.Split("\n").FirstOrDefault() ?? string.Empty;
        var headerTokens = firstLine.Split(" ");

        var headerErrors = new List<string>();

        if (headerTokens.Length != 5) {
            headerErrors.Add("Header must be comment with Elan, version, status and hash");
        }
        else {
            if (headerTokens[0].Trim() != "#") {
                headerErrors.Add("Header is not comment");
            }

            if (headerTokens[2].Trim() == "Elan") {
                var codeBody = code[(code.IndexOf("Elan"))..];
                if (!ValidateHash(headerTokens[1].Trim(), codeBody)) {
                    headerErrors.Add("Hash check failed");
                }
            }
            else {
                headerErrors.Add("Incorrect language expect: 'Elan'");
            }

            if (headerTokens[3].Trim() != "v0.1") {
                headerErrors.Add("Incorrect Elan version expect: 'v0.1'");
            }

            if (headerTokens[4].Trim() != "valid") {
                headerErrors.Add("Status must be 'valid'");
            }
        }

        return headerErrors.Count == 0 ? compileData with { ElanCode = code } : compileData with { ElanCode = "", HeaderErrors = headerErrors.ToArray() };
    }

    private static bool ValidateHash(string headerToken, string code) {
        if (headerToken == "FFFFFFFFFFFFFFFF") {
            // debug test value
            return true;
        }

        var normalizedCode = code.Trim().Replace("\r", "");

        using var sha256 = SHA256.Create();
        var bs = Encoding.UTF8.GetBytes(normalizedCode);

        var hash = sha256.ComputeHash(bs);
        var hexHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

        return hexHash == headerToken;
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
        if (compileData.AbstractSyntaxTree is null || compileData.CompileErrors.Length > 0) {
            return compileData;
        }

        var codeVisitor = new CodeModelAstVisitor(compileData.SymbolTable?.GlobalScope ?? throw new NullReferenceException());
        var codeModel = codeVisitor.Visit(compileData.AbstractSyntaxTree);
        var objectCode = codeModel.ToString() ?? "";

        return compileData with { ObjectCode = objectCode };
    }

    private static CompileData CompileObjectCode(CompileData compileData) {
        if (!compileData.CompileOptions.CompileToCSharp || string.IsNullOrWhiteSpace(compileData.ObjectCode)) {
            return compileData;
        }

        var (fileName, stdOut, stdErr) = CSharpCompiler.CompileObjectCode(compileData.FileName, compileData.ObjectCode, compileData.WorkingDirectory);

        return compileData with { FileName = fileName, ObjectCodeCompileStdOut = stdOut, ObjectCodeCompileStdErr = stdErr };
    }

    private static CompileData GenerateSymbolTable(CompileData compileData) {
        if (compileData.AbstractSyntaxTree is null) {
            return compileData;
        }

        var symbolTableVisitor = new SymbolTableVisitor();
        symbolTableVisitor.Visit(compileData.AbstractSyntaxTree);

        if (symbolTableVisitor.SymbolErrors.Any()) {
            return compileData with { CompileErrors = symbolTableVisitor.SymbolErrors.ToArray() };
        }

        symbolTableVisitor.Pass = VisitorPass.Second;

        symbolTableVisitor.Visit(compileData.AbstractSyntaxTree);

        symbolTableVisitor.Validate();

        if (symbolTableVisitor.SymbolErrors.Any()) {
            return compileData with { CompileErrors = symbolTableVisitor.SymbolErrors.ToArray() };
        }

        return compileData with { SymbolTable = symbolTableVisitor.SymbolTable };
    }

    private static CompileData SecondPass(CompileData compileData) {
        if (compileData.AbstractSyntaxTree is null || compileData.SymbolTable is null) {
            return compileData;
        }

        var secondPassVisitor = new SecondPassVisitor(compileData.SymbolTable);
        var ast = secondPassVisitor.Visit(compileData.AbstractSyntaxTree);

        return compileData with { AbstractSyntaxTree = ast };
    }

    private static CompileData ThirdPass(CompileData compileData) {
        if (compileData.AbstractSyntaxTree is null || compileData.SymbolTable is null) {
            return compileData;
        }

        var thirdPassVisitor = new ThirdPassVisitor(compileData.SymbolTable);
        var ast = thirdPassVisitor.Visit(compileData.AbstractSyntaxTree);

        return compileData with { AbstractSyntaxTree = ast, CompileErrors = thirdPassVisitor.CompileErrors.ToArray() };
    }
}