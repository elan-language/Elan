using Compiler;
namespace Test.CompilerTests;
using static Helpers;

[TestClass]
public class T38_ExpressionsSplitOverMoreThanOneLine
{
    #region Passes
    [TestMethod]
    public void Pass_1()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 +
     cos(x) ^ 2
  printLine(y)
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
    var x = 0.7;
    var y = System.Math.Pow(sin(x), 2) + System.Math.Pow(cos(x), 2);
    printLine(y);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalValue 0.7))))) (varDef var (assignableValue y) = (expression (expression (expression (methodCall sin ( (argumentList (expression (value x))) ))) ^ (expression (value (literal (literalValue 2))))) (binaryOp (arithmeticOp +)) (expression (expression (expression (methodCall cos ( (argumentList (expression (value x))) ))) ^ (expression (value (literal (literalValue 2)))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_2()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 
       2 + cos(x) ^ 2
  printLine(y)
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
    var x = 0.7;
    var y = System.Math.Pow(sin(x), 2) + System.Math.Pow(cos(x), 2);
    printLine(y);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalValue 0.7))))) (varDef var (assignableValue y) = (expression (expression (expression (methodCall sin ( (argumentList (expression (value x))) ))) ^ (expression (value (literal (literalValue 2))))) (binaryOp (arithmeticOp +)) (expression (expression (methodCall cos ( (argumentList (expression (value x))) ))) ^ (expression (value (literal (literalValue 2))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_3()
    {
        var code = @"#
main
  var x = 0.7
  var y = 
sin(x) ^ 2 + cos(x) ^ 2
  printLine(y)
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
    var x = 0.7;
    var y = System.Math.Pow(sin(x), 2) + System.Math.Pow(cos(x), 2);
    printLine(y);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalValue 0.7))))) (varDef var (assignableValue y) = (expression (expression (expression (expression (methodCall sin ( (argumentList (expression (value x))) ))) ^ (expression (value (literal (literalValue 2))))) (binaryOp (arithmeticOp +)) (expression (expression (methodCall cos ( (argumentList (expression (value x))) ))) ^ (expression (value (literal (literalValue 2)))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_4()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(
        x) ^ 2  + cos(x) ^ 2
  printLine(y)
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
    var x = 0.7;
    var y = System.Math.Pow(sin(x), 2) + System.Math.Pow(cos(x), 2);
    printLine(y);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalValue 0.7))))) (varDef var (assignableValue y) = (expression (expression (expression (methodCall sin ( (argumentList (expression (expression (value x)))) ))) ^ (expression (value (literal (literalValue 2))))) (binaryOp (arithmeticOp +)) (expression (expression (methodCall cos ( (argumentList (expression (value x))) ))) ^ (expression (value (literal (literalValue 2))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_5()
    {
        var code = @"#
main
  var x = 0.7
  var y = 3 + 4 *
    1 + 2
  printLine(y)
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
    var x = 0.7;
    var y = 3 + 4 * 1 + 2;
    printLine(y);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalValue 0.7))))) (varDef var (assignableValue y) = (expression (expression (expression (value (literal (literalValue 3)))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 4))))) (binaryOp (arithmeticOp *)) (expression (expression (expression (value (literal (literalValue 1)))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 2)))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) ))))) end main) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "9\r\n");
    }

    #endregion

    #region Fails
    [TestMethod]
    public void Fail_1()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 
     + cos(x) ^ 2
  printLine(y)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

  

    [TestMethod]
    public void Fail_2()
    {
        var code = @"#
main
  var x = 0.7
  var y 
     = sin(x) ^ 2  + cos(x) ^ 2
  printLine(y)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
    #endregion
}