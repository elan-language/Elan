using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_3_NoStartingCommentSymbol {
    [TestMethod]
    public void Pass1() {
        var code = @"main
  printLine(""Hello World!"")
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(""Hello World!"");
  }
}";

        var parseTree = @"(file (main  main (statementBlock (callStatement  (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""Hello World!""))))) )))))  end main)  <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }

    [TestMethod]
    public void Pass2() {
        var code = @"## a comment
main
  printLine(""Hello World!"")
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(""Hello World!"");
  }
}";

        var parseTree = @"(file (main  main (statementBlock (callStatement  (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""Hello World!""))))) )))))  end main)  <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }
}