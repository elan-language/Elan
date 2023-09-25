using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T29_Expressions3_FunctionCalls
{
    [TestMethod]
    public void Pass_LibraryConst() {
        var code = @"#
main
  printLine(pi)
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
    printLine(pi);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value pi))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3.141592653589793\r\n");
    }

    [TestMethod]        
    public void Pass_SingleFunctionCall()
    {
        var code = @"#
main
  var x = sin(pi/180*30)
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
    var x = sin(Compiler.WrapperFunctions.FloatDiv(pi, 180) * 30);
    printLine(x);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (methodCall sin ( (argumentList (expression (expression (expression (value pi)) (binaryOp (arithmeticOp /)) (expression (value (literal (literalValue 180))))) (binaryOp (arithmeticOp *)) (expression (value (literal (literalValue 30)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value x))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0.49999999999999994\r\n");
    }

    [TestMethod]
    public void Pass_DotSyntax()
    {
        var code = @"#
main
  var x =  pi/180*30
  var y = x.sin()
  printLine(y)
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
    var x = Compiler.WrapperFunctions.FloatDiv(pi, 180) * 30;
    var y = sin(x);
    printLine(y);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (expression (expression (value pi)) (binaryOp (arithmeticOp /)) (expression (value (literal (literalValue 180))))) (binaryOp (arithmeticOp *)) (expression (value (literal (literalValue 30)))))) (varDef var (assignableValue y) = (expression (expression (value x)) . (methodCall sin ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0.49999999999999994\r\n");
    }


    [TestMethod]
    public void Pass_DotSyntaxFunctionEvaluationHasPrecedenceOverOperators()
    {
        var code = @"#
main
  var x =  pi/180*30
  var y = 2 + x.sin()
  printLine(y)
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
    var x = Compiler.WrapperFunctions.FloatDiv(pi, 180) * 30;
    var y = 2 + sin(x);
    printLine(y);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (expression (expression (value pi)) (binaryOp (arithmeticOp /)) (expression (value (literal (literalValue 180))))) (binaryOp (arithmeticOp *)) (expression (value (literal (literalValue 30)))))) (varDef var (assignableValue y) = (expression (expression (value (literal (literalValue 2)))) (binaryOp (arithmeticOp +)) (expression (expression (value x)) . (methodCall sin ( ))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2.5\r\n"); //Add full digits
    }

    [TestMethod]
    public void Pass_MoreComplexExpression()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 + cos(x) ^ 2
  printLine(y)
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


    [TestMethod, Ignore]
    public void Fail_UnconsumedExpressionResult1()
    {
        var code = @"#
    main
      sin(1)
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_UnconsumedExpressionResult2()
    {
        var code = @"#
    main
      1 + 2
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_UnconsumedExpressionResult3()
    {
        var code = @"#
    main
      var a = 1
      a.Sin()
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData);
    }

}