using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Compiler;

namespace Test.CompilerTests;

public static partial class Helpers {
    // because now windows newlines on appveyor
    public static readonly string NL = Environment.GetEnvironmentVariable("APPVEYOR") is "True" ? @"\n" : @"\r\n";

    [GeneratedRegex("\\s+")]
    private static partial Regex WsRegex();

    public static string CollapseWs(string inp) => WsRegex().Replace(inp.Replace("\\r", "").Replace("\\n", ""), " ");

    public static string NormalizeNewLines(string inp) => inp.Replace("\r", "");

    private static Process CreateProcess(string exe, string workingDir) {
        var start = new ProcessStartInfo {
            FileName = exe,
            Arguments = "",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardInputEncoding = Encoding.Default,
            StandardOutputEncoding = Encoding.Default,
            WorkingDirectory = workingDir,
            CreateNoWindow = false,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        return Process.Start(start) ?? throw new NullReferenceException("Process failed to start");
    }

    private static string Execute(string exe, string workingDir) {
        using var process = CreateProcess(exe, workingDir);

        if (!process.WaitForExit(20000)) {
            process.Kill();
        }

        using var stdOut = process.StandardOutput;
        return stdOut.ReadToEnd();
    }

    public static void AssertParses(CompileData compileData) {
        Assert.IsTrue(compileData.ParserErrors.Length == 0, "Unexpected parse error");
    }

    public static void AssertParseTreeIs(CompileData compileData, string expectedParseTree) {
        if (expectedParseTree is "*" && !string.IsNullOrWhiteSpace(compileData.ParseStringTree)) {
            return;
        }

        Assert.AreEqual(CollapseWs(expectedParseTree), CollapseWs(compileData.ParseStringTree));
    }

    public static void AssertCompiles(CompileData compileData) {
        Assert.IsNotNull(compileData.AbstractSyntaxTree, "Failed to compile");
    }

    public static void AssertObjectCodeIs(CompileData compileData, string objectCode) {
        Assert.AreEqual(NormalizeNewLines(objectCode), NormalizeNewLines(compileData.ObjectCode));
    }

    public static void AssertObjectCodeCompiles(CompileData compileData) {
        Assert.IsTrue(compileData.ObjectCodeCompileStdOut.Contains("Build succeeded."), CollapseWs(compileData.ObjectCodeCompileStdOut));
    }

    public static void AssertObjectCodeDoesNotCompile(CompileData compileData) {
        Assert.IsFalse(compileData.ObjectCodeCompileStdOut.Contains("Build succeeded."), "Unexpectedly compiled object code");
    }

    public static void AssertObjectCodeExecutes(CompileData compileData, string expectedOutput) {
        var wd = $@"{Directory.GetCurrentDirectory()}\obj\bin\Debug\net7.0";
        var exe = $@"{wd}\{Path.GetFileNameWithoutExtension(compileData.FileName)}.exe";

        var stdOut = Execute(exe, wd);

        Assert.AreEqual(expectedOutput, stdOut);
    }

    public static void AssertDoesNotParse(CompileData compileData, string[]? expected = null) {
        Assert.IsTrue(compileData.ParserErrors.Length > 0, "Expected parse error");

        var errors = compileData.ParserErrors.Select(e => e.Message).ToArray();

        var zip = expected?.Zip(errors);

        foreach (var (e, a) in zip ?? Array.Empty<(string, string)>()) {
            if (e is not "*") {
                Assert.AreEqual(e, a);
            }
        }
    }

    public static void AssertDoesNotCompile(CompileData compileData) {
        Assert.IsNull(compileData.AbstractSyntaxTree, "Unexpectedly Compiled");
    }
}