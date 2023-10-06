using Compiler;
namespace Test.CompilerTests;
using static Helpers;

[TestClass, Ignore]
public class T17_ThrowAndCatchException
{
    #region Passes
    [TestMethod]
    public void Pass_ThrowExceptionInMain()
    {
        var code = @"
main
    throw new Exception(""Foo"")
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
    throw new Exception(""Foo"");
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "?");//Exception thrown with message Foo
    }

    [TestMethod]
    public void Pass_ThrowExceptionInProcedure()
    {
        var code = @"
main
   foo()
end main

procedure foo()
  throw new Exception(""Foo"")
end procedure
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
    foo();
  }

  private static void foo() {
    throw new Exception(""Foo"");
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "?");//Exception thrown
    }

    [TestMethod]
    public void Pass_ThrowExceptionInFunction()
    {
        var code = @"
main
   foo()
end main

function foo(x Int) as Int
  throw new Exception(""Foo"")
end function
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
    var x = foo(1);
  }

  private static int foo() {
    throw new Exception(""Foo"");
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "?");//Exception thrown
    }

    [TestMethod]
    public void Pass_CatchException()
    {
        var code = @"
main
  try
     foo()
     printLine(""not caught"")
  catch e Exception
    printLine(""caught"")
  end try
end main

procedure foo()
  throw new Exception(""Foo"")
end procedure
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
      foo();
      printLine(""not caught"");
   } catch Exception e {
   printLine(""caught"");
}
  }

  private static void foo() {
    throw new Exception(""Foo"");
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "caught\r\n");
    }

    [TestMethod]
    public void Pass_CatchMoreSpecificException()
    {
        var code = @"
main
  try
     var x = 1
     var y = 0
     var z = x / y
     printLine(""not caught"")
  catch e Exception
    printLine(""caught"")
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
      printLine(""not caught"");
   } catch Exception e {
   printLine(""caught"");
   }
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "caught\r\n");
    }

    [TestMethod]
    public void Pass_DoesNotCatchMoreGeneralException()
    {
        var code = @"
main
  try
     foo()
     printLine(""not caught"")
  catch e DivisionByZeroException
    printLine(""caught"")
  end try
end main

procedure foo()
  throw new Exception(""Foo"")
end procedure
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
      foo();
      printLine(""not caught"");
   } catch DivisionByZero e {
   printLine(""caught"");
}
  }

  private static void foo() {
    throw new Exception(""Foo"");
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "not caught\r\n");
    }
    #endregion

    #region Fails
    [TestMethod]
    public void Fail_MissingNew()
    {
        var code = @"
main
    throw Exception(""Foo"")
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_MissingThrow()
    {
        var code = @"
main
    new Exception(""Foo"")
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
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
  catch Exception
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

    [TestMethod]
    public void Fail_catchMissingType()
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
  throw new Exception(""Foo"")
end procedure
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
    #endregion
}