﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T_3_NoBlankLine
    {

    [TestMethod]
    public void Pass1()
    {
        var code = @"main
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

        var parseTree = @"(file (main \r\n main (statementBlock (callStatement \r\n (expression (methodCall printLine ( (argumentList (expression (value (literalValue ""Hello World!"")))) ))))) \r\n end main) \r\n <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }
}

