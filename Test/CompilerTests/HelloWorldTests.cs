﻿using Compiler;
using System.Linq.Expressions;

namespace Test.CompilerTests;

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

        var parseTree = $@"(file (main {NL} main (statementBlock (callStatement {NL} (expression (methodCall printLine ( (argumentList (expression (value (literalValue ""Hello World!"")))) ))))) {NL} end main) {NL} <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, @"Hello World!
");
    }

    [TestMethod]
    public void Pass2() {
        var code = @"
main
  printLine(1)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static StandardLibrary.SystemCalls;

public static class Program {
    private static void Main(string[] args) {
      printLine(1);
    }
}";

        var parseTree = $@"(file (main {NL} main (statementBlock (callStatement {NL} (expression (methodCall printLine ( (argumentList (expression (value (literalValue 1)))) ))))) {NL} end main) {NL} <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, @"1
");
    }

    [TestMethod]
    public void Pass3() {
        var code = @"
main
  printLine(2.1)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static StandardLibrary.SystemCalls;

public static class Program {
    private static void Main(string[] args) {
      printLine(2.1);
    }
}";

        var parseTree = $@"(file (main {NL} main (statementBlock (callStatement {NL} (expression (methodCall printLine ( (argumentList (expression (value (literalValue 2.1)))) ))))) {NL} end main) {NL} <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, @"2.1
");
    }

    [TestMethod]
    public void Pass4() {
        var code = @"
main
  printLine('%')
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static StandardLibrary.SystemCalls;

public static class Program {
    private static void Main(string[] args) {
      printLine('%');
    }
}";

        var parseTree = $@"(file (main {NL} main (statementBlock (callStatement {NL} (expression (methodCall printLine ( (argumentList (expression (value (literalValue '%')))) ))))) {NL} end main) {NL} <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, @"%
");
    }

    [TestMethod]
    public void Pass5() {
        var code = @"
main
  printLine(true)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static StandardLibrary.SystemCalls;

public static class Program {
    private static void Main(string[] args) {
      printLine(true);
    }
}";

        var parseTree = $@"(file (main {NL} main (statementBlock (callStatement {NL} (expression (methodCall printLine ( (argumentList (expression (value (literalValue true)))) ))))) {NL} end main) {NL} <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, @"True
");
    }

    [TestMethod]
    public void Pass6() {
        var code = @"
main
  printLine()
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static StandardLibrary.SystemCalls;

public static class Program {
    private static void Main(string[] args) {
      printLine();
    }
}";

        var parseTree = $@"(file (main {NL} main (statementBlock (callStatement {NL} (expression (methodCall printLine ( ))))) {NL} end main) {NL} <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, @"
");
    }

    [TestMethod]
    public void Fail1() {
        var code = @"
  printLine(""Hello World`)
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail2() {
        var code = @"
main
  printLine(""Hello World!"")

";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail3() {
        var code = @"
main
  printline(""Hello World!"")

";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail4() {
        var code = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData);
    }
}