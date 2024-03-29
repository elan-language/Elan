﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using Compiler;

namespace Test.CompilerTests;

public static partial class Helpers {

    public static bool LightweightTest { get; } = false;

    private static bool IsAppveyor => Environment.GetEnvironmentVariable("APPVEYOR") is "True";

    [GeneratedRegex("\\s+")]
    private static partial Regex WsRegex();

    private static string CollapseWs(string inp) => WsRegex().Replace(inp.Replace("\\r", "").Replace("\\n", ""), " ");

    private static string NormalizeNewLines(string inp) => inp.Replace("\r", "");

    private static Process CreateProcess(string exe, string workingDir, string args) {
        var start = new ProcessStartInfo {
            FileName = exe,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardInputEncoding = null,
            StandardOutputEncoding = null,
            WorkingDirectory = workingDir,
            CreateNoWindow = false,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        return Process.Start(start) ?? throw new NullReferenceException("Process failed to start");
    }

    private static (string, string) Execute(string exe, string workingDir, string args, string? input = null) {
        using var process = CreateProcess(exe, workingDir, args);

        if (input is not null) {
            using var stdIn = process.StandardInput;
            stdIn.WriteLine(input);
        }

        if (!process.WaitForExit(20000)) {
            process.Kill();
        }

        using var stdOut = process.StandardOutput;
        using var stdErr = process.StandardError;
        return (stdOut.ReadToEnd(), stdErr.ReadToEnd());
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
        Assert.IsTrue(compileData.CompileErrors.Length == 0, $"Failed to compile :\r\n {string.Join("\r\n", compileData.CompileErrors)}");
    }

    public static void AssertObjectCodeIs(CompileData compileData, string objectCode) {
        Assert.AreEqual(NormalizeNewLines(objectCode), NormalizeNewLines(compileData.ObjectCode));
    }

    public static void AssertObjectCodeCompiles(CompileData compileData) {
        Assert.IsTrue(LightweightTest || compileData.ObjectCodeCompileStdOut.Contains("Build succeeded."), CollapseWs(compileData.ObjectCodeCompileStdOut));
    }

    public static void AssertObjectCodeDoesNotCompile(CompileData compileData) {
        Assert.IsFalse(compileData.ObjectCodeCompileStdOut.Contains("Build succeeded."), "Unexpectedly compiled object code");
    }

    public static void AssertObjectCodeExecutes(CompileData compileData, string expectedOutput, string? optionalInput = null) {
        if (!LightweightTest) {
            var wd = $@"{Directory.GetCurrentDirectory()}\obj\bin\Debug\net7.0";
            var exe = $@"{wd}\{Path.GetFileNameWithoutExtension(compileData.FileName)}.exe";

            var (stdOut, stdErr) = Execute(exe, wd, "", optionalInput);

            Assert.AreEqual(NormalizeNewLines(expectedOutput), NormalizeNewLines(stdOut));
        }
    }

    public static void AssertObjectCodeExecutesTests(CompileData compileData, string expectedOutput, string? optionalInput = null) {
        if (!LightweightTest) {
            var wd = $@"{Directory.GetCurrentDirectory()}\obj\bin\Debug\net7.0";
            var exe = $@"dotnet";
            var args = $@"test {wd}\{Path.GetFileNameWithoutExtension(compileData.FileName)}.exe";

            var (stdOut, stdErr) = Execute(exe, wd, args, optionalInput);

            Assert.IsTrue(stdOut.Contains(expectedOutput), stdOut);
        }
    }

    public static void AssertObjectCodeFails(CompileData compileData, string expectedError, string? optionalInput = null) {
        if (!LightweightTest) {
            var wd = $@"{Directory.GetCurrentDirectory()}\obj\bin\Debug\net7.0";
            var exe = $@"{wd}\{Path.GetFileNameWithoutExtension(compileData.FileName)}.exe";

            var (stdOut, stdErr) = Execute(exe, wd, "",  optionalInput);

            Assert.IsTrue(stdErr.Contains(expectedError));
        }
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

    public static void AssertDoesNotCompile(CompileData compileData, string messageStarting) {
        Assert.IsTrue(compileData.CompileErrors.Length > 0, "Unexpectedly Compiled");
        var actual = compileData.CompileErrors[0];
        if (messageStarting != "*" && !actual.StartsWith(messageStarting)) {
            Assert.AreEqual(messageStarting, actual);
        }
    }

    public static void AssertInvalidHeader(CompileData compileData, string messageStarting) {
        Assert.IsTrue(compileData.HeaderErrors.Length > 0, "Unexpectedly valid header");
        var actual = compileData.HeaderErrors[0];
        if (messageStarting != "*" && !actual.StartsWith(messageStarting)) {
            Assert.AreEqual(messageStarting, actual);
        }
    }

    public static string ReadCodeFile(string fileName) => File.ReadAllText($"ExampleProjects\\{fileName}");

    public static void CleanUpArtifacts() {
        var wd = $@"{Directory.GetCurrentDirectory()}\obj";

        if (Directory.Exists(wd)) {
            Directory.Delete(wd, true);
        }
    }

    public static CompileData CompileData(string elanCode) {
        return new CompileData() { ElanCode = elanCode, CompileOptions = new CompileOptions() { CompileToCSharp = !LightweightTest } };
    }

}