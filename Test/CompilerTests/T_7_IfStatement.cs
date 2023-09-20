using Compiler;

namespace Test.CompilerTests;

using static Helpers;

// Ticket #2
[TestClass]
public class T_7_IfStatement {
    [TestMethod]
    public void Pass1() {
        var code = @"#
main
  var a = true
  if a then
    printLine(""yes"")
  else
    printLine(""no"")
  end if
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
    var a = true;
    if (a) {
      printLine(""yes"");
    }
    else {
      printLine(""no"");
    }
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalValue true))))) (proceduralControlFlow (if if (expression (value a)) then (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""yes""))))) ))))) else (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""no""))))) ))))) end if))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "yes\r\n");

    }

    
}