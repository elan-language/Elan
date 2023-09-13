using Compiler;
using System.Diagnostics;
using System.Text;

namespace Test.CompilerTests;

public static class Helpers {

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

        if (!process.WaitForExit(10000)) {
            process.Kill();
        }

        using var stdOut = process.StandardOutput;
        return stdOut.ReadToEnd();
    }

    public static void AssertParses(CompileData compileData) {
        Assert.IsTrue(compileData.ParserErrors.Length == 0, "Unexpected parse error");
    }

    public static void AssertParseTreeIs(CompileData compileData, string expectedParseTree) {
        Assert.AreEqual(expectedParseTree, compileData.ParseStringTree);
    }

    public static void AssertCompiles(CompileData compileData) {
        Assert.IsNotNull(compileData.AbstractSyntaxTree, "Failed to compile");
    }

    public static void AssertObjectCodeIs(CompileData compileData, string objectCode) {
        Assert.AreEqual(objectCode, compileData.ObjectCode);
    }

    public static void AssertObjectCodeCompiles(CompileData compileData) {
        Assert.IsTrue(compileData.ObjectCodeCompileStdOut.Contains("Build succeeded."), "Failed to compile object code");
    }


    public static void AssertObjectCodeExecutes(CompileData compileData, string expectedOutput) {
        var wd = $@"{Directory.GetCurrentDirectory()}\obj\bin\Debug\net7.0";
        var exe = $@"{wd}\{Path.GetFileNameWithoutExtension(compileData.FileName)}.exe";

        var stdOut = Execute(exe, wd);

        Assert.AreEqual(expectedOutput, stdOut);
    }

    public static void AssertDoesNotParse(CompileData compileData) {
        Assert.IsTrue(compileData.ParserErrors.Length > 0, "Expected parse error");
    }
}