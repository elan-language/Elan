using Compiler;

namespace Test.CompilerTests;

using static Helpers;


[TestClass]
public class T11_RepeatUntil
{
    [TestMethod]
    public void Pass_minimal()
    {
        var code = @"
main
   var x = 0
   repeat
     x = x + 1
   until  x >= 10
   printLine(x)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0;
    do {
      x = x + 1;
    } while (!(x >= 10));
    printLine(x);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (repeat repeat (statementBlock (assignment (assignableValue x) = (expression (expression (value x)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))))) until (expression (expression (value x)) (binaryOp (conditionalOp >=)) (expression (value (literal (literalValue 10))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value x))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_innerLoop()
    {
        var code = @"
main
   var t = 0
   var x = 0
   repeat
    var y = 0
       repeat
         y = y + 1
         t = t + 1
       until  y > 4
     x = x + 1
   until  x > 3
   printLine(t)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var t = 0;
    var x = 0;
    do {
      var y = 0;
      do {
        y = y + 1;
        t = t + 1;
      } while (!(y > 4));
      x = x + 1;
    } while (!(x > 3));
    printLine(t);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue t) = (expression (value (literal (literalValue 0))))) (varDef var (assignableValue x) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (repeat repeat (statementBlock (varDef var (assignableValue y) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (repeat repeat (statementBlock (assignment (assignableValue y) = (expression (expression (value y)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1)))))) (assignment (assignableValue t) = (expression (expression (value t)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))))) until (expression (expression (value y)) (binaryOp (conditionalOp >)) (expression (value (literal (literalValue 4))))))) (assignment (assignableValue x) = (expression (expression (value x)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))))) until (expression (expression (value x)) (binaryOp (conditionalOp >)) (expression (value (literal (literalValue 3))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value t))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "20\r\n");
    }

    [TestMethod]
    public void Fail_noUntil()
    {
        var code = @"
main
   var x = 0
   repeat
     x = x + 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_variableRedeclaredInTest()
    {
        var code = @"
main
    var x = 0
    repeat
      x = x + 1
   until var x >= 10
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_variableDefinedInLoop()
    {
        var code = @"
main
   repeat
     var x = x + 1
   until  x >= 10
end main
";

      
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }


    [TestMethod]
    public void Fail_testPutOnRepeat()
    {
        var code = @"
main
    var x = 0
    repeat x >= 10
      x = x + 1
    until 
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

 
    [TestMethod]
    public void Fail_noCondition()
    {
        var code = @"
main
    var x = 0
    repeat
      x = x + 1
    until 
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_invalidCondition()
    {
        var code = @"
main
    var x = 0
    repeat
      x = x + 1
    until >= 10
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

}