using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T10_WhileLoop {
    [TestMethod]
    public void Pass_minimal() {
        var code = @"
main
   var x = 0
   while x < 10
     x = x + 1
   end while
   printLine(x)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0;
    while (x < 10) {
      x = x + 1;
    }
    printLine(x);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (while while (expression (expression (value x)) (binaryOp (conditionalOp <)) (expression (value (literal (literalValue 10))))) (statementBlock (assignment (assignableValue x) = (expression (expression (value x)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))))) end while)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value x))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_innerLoop() {
        var code = @"
main
    var t = 0
    var x = 0
    while x < 3
        var y = 0
        while y < 4
            y = y + 1
            t = t + 1
        end while
        x = x + 1
    end while
   printLine(t)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var t = 0;
    var x = 0;
    while (x < 3) {
      var y = 0;
      while (y < 4) {
        y = y + 1;
        t = t + 1;
      }
      x = x + 1;
    }
    printLine(t);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue t) = (expression (value (literal (literalValue 0))))) (varDef var (assignableValue x) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (while while (expression (expression (value x)) (binaryOp (conditionalOp <)) (expression (value (literal (literalValue 3))))) (statementBlock (varDef var (assignableValue y) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (while while (expression (expression (value y)) (binaryOp (conditionalOp <)) (expression (value (literal (literalValue 4))))) (statementBlock (assignment (assignableValue y) = (expression (expression (value y)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1)))))) (assignment (assignableValue t) = (expression (expression (value t)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))))) end while)) (assignment (assignableValue x) = (expression (expression (value x)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))))) end while)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value t))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Fail_noEnd() {
        var code = @"
main
   var x = 0
   while x < 10
     x = x + 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_variableNotPredefined() {
        var code = @"
main
   while x < 10
     x = x + 1
   end while
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_variableDefinedInWhile() {
        var code = @"
main
   while var x < 10
     x = x + 1
   end while
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noCondition() {
        var code = @"
main
   var x = 0
   while
     x = x + 1
   end while
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_while_do()
    {
        var code = @"
main
   var x = 0
   while x < 10
     x = x + 1
   do
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}