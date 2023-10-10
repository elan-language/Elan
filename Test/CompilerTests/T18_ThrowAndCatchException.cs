using Compiler;
namespace Test.CompilerTests;
using static Helpers;

[TestClass]
public class T18_ThrowAndCatchException
{
    #region Passes
    [TestMethod]
    public void Pass_ThrowExceptionInMain()
    {
        var code = @"
main
    throwException(""Foo"")
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    throwException(@$""Foo"");
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall throwException ( (argumentList (expression (value (literal (literalDataStructure ""Foo""))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. StandardLibrary.ElanException: Foo");
    }

    [TestMethod]
    public void Pass_ThrowExceptionInProcedure()
    {
        var code = @"
main
   foo()
end main

procedure foo()
  throwException(""Foo"")
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {
  public static void foo() {
    throwException(@$""Foo"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    foo();
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall foo ( ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall throwException ( (argumentList (expression (value (literal (literalDataStructure ""Foo""))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. StandardLibrary.ElanException: Foo");
    }

    [TestMethod]
    public void Pass_CatchException()
    {
        var code = @"
main
  try
     foo()
     printLine(""not caught"")
  catch e
    printLine(""caught"")
  end try
end main

procedure foo()
  throwException(""Foo"")
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {
  public static void foo() {
    throwException(@$""Foo"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    try {
      foo();
      printLine(@$""not caught"");
    }
    catch (Exception _e) {
      var e = new StandardLibrary.ElanException(_e);
      printLine(@$""caught"");
    }
  }
}";

        var parseTree = @"(file (main main (statementBlock (proceduralControlFlow (try try (statementBlock (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""not caught""))))) ))))) catch e (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""caught""))))) ))))) end try))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall throwException ( (argumentList (expression (value (literal (literalDataStructure ""Foo""))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "caught\r\n");
    }

    [TestMethod]
    public void Pass_CatchSystemGeneratedException()
    {
        var code = @"
main
  try
     var x = 1
     var y = 0
     var z = x div y
     printLine(@$""not caught"");
  catch e
    printLine(e)
  end try
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    try {
      var x = 1;
      var y = 0;
      var z = x / y;
      printLine(@$""not caught"");
    }
    catch (Exception _e) {
      var e = new StandardLibrary.ElanException(_e);
      printLine(e);
    }
  }
}";

        var parseTree = @"(file (main main (statementBlock (proceduralControlFlow (try try (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalValue 1))))) (varDef var (assignableValue y) = (expression (value (literal (literalValue 0))))) (varDef var (assignableValue z) = (expression (expression (value x)) (binaryOp (arithmeticOp div)) (expression (value y)))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""not caught""))))) ))))) catch e (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value e))) ))))) end try))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Attempted to divide by zero.\r\n"); 
    }

    [TestMethod]
    public void Pass_UseException()
    {
        var code = @"
main
  try
     foo()
     printLine(""not caught"")
  catch e
    printLine(e.message)
  end try
end main

procedure foo()
  throwException(""Foo"")
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {
  public static void foo() {
    throwException(@$""Foo"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    try {
      foo();
      printLine(@$""not caught"");
    }
    catch (Exception _e) {
      var e = new StandardLibrary.ElanException(_e);
      printLine(e.message);
    }
  }
}";

        var parseTree = @"(file (main main (statementBlock (proceduralControlFlow (try try (statementBlock (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""not caught""))))) ))))) catch e (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value e)) . message)) ))))) end try))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall throwException ( (argumentList (expression (value (literal (literalDataStructure ""Foo""))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Foo\r\n");
    }

    #endregion

    #region Fails


    [TestMethod]
    public void Fail_ThrowExceptionInFunction()
    {
        var code = @"
main
   var s = foo()
end main

function foo(x String) as String
  throwException(x)
  return x
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData);
    }
    [TestMethod]
    public void Fail_catchMissingVariable()
    {
        var code = @"
main
  try
     foo()
     printLine(""not caught"")
  catch
    printLine(""caught"")
  end try
end main

procedure foo()
  throw new Exception(""Foo"")
end procedure
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
    #endregion
}