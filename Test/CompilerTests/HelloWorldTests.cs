namespace Test.CompilerTests;

using Compiler;
using static Helpers;

// Ticket #2
[TestClass]
public class HelloWorldTests {
    [TestMethod]
    public void Pass1() {
        var code = @"
main
  printLine(""Hello World!"")
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static StandardLibrary.SystemCalls;

public static class Program {
    private static void Main(string[] args) {
      printLine(""Hello World!"");
    }
}";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });

        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
    }

    [TestMethod, Ignore]
    public void Pass2() {
        var code = @"
main
  printLine(1)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
    }

    [TestMethod, Ignore]
    public void Pass3() {
        var code = @"
main
  printLine(2.1)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
    }

    [TestMethod, Ignore]
    public void Pass4() {
        var code = @"
main
  printLine('%')
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
    }

    [TestMethod, Ignore]
    public void Pass5() {
        var code = @"
main
  printLine(true)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
    }

    [TestMethod, Ignore]
    public void Pass6() {
        var code = @"
main
  printLine()
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
    }

    [TestMethod, Ignore]
    public void Fail1() {
        var code = @"
  printLine(""Hello World`)
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Fail2() {
        var code = @"
main
  printLine(""Hello World!"")

";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Fail3() {
        var code = @"
main
  printline(""Hello World!"")

";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Fail4() {
        var code = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}