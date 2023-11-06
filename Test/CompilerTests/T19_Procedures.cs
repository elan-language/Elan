using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T19_Procedures {
    #region Passes

    [TestMethod]
    public void Pass_BasicOperationIncludingSystemCall() {
        var code = @"
main
    printLine(1)
    foo()
    printLine(3)
end main

procedure foo()
    printLine(2)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    printLine(2);
  }
}

public static class Program {
  private static void Main(string[] args) {
    printLine(1);
    foo();
    printLine(3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n2\r\n3\r\n");
    }

    [TestMethod]
    public void Pass_WithParamsPassingVariables() {
        var code = @"
main
    var a = 2
    var b = ""hello""
    foo(a, b)
end main

procedure foo(a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a, ref string b) {
    printLine(a);
    printLine(b);
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 2;
    var b = @$""hello"";
    foo(ref a, ref b);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalValue 2))))) (varDef var (assignableValue b) = (expression (value (literal (literalDataStructure ""hello""))))) (callStatement (expression (methodCall foo ( (argumentList (expression (value a)) , (expression (value b))) ))))) end main) (procedureDef procedure (procedureSignature foo ( (parameterList (parameter a (type Int)) , (parameter b (type String))) )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value b))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello\r\n");
    }

    [TestMethod]
    public void Pass_WithParamsPassingLiteralsOrExpressions() {
        var code = @"
main
    var a = 1
    foo(a + 1, ""hello"")
end main

procedure foo(a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a, ref string b) {
    printLine(a);
    printLine(b);
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 1;
    var _foo_0 = a + 1;
    var _foo_1 = @$""hello"";
    foo(ref _foo_0, ref _foo_1);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalValue 1))))) (callStatement (expression (methodCall foo ( (argumentList (expression (expression (value a)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))) , (expression (value (literal (literalDataStructure ""hello""))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( (parameterList (parameter a (type Int)) , (parameter b (type String))) )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value b))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello\r\n");
    }

    [TestMethod]
    public void Pass_paramsCanBeUpdated() {
        var code = @"
main
    var a = 1
    var b = ""hello""
    foo(a, b)
    printLine(a)
    printLine(b)
end main

procedure foo (a Int, b String)
    a = a + 1
    b = b + ""!""
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a, ref string b) {
    a = a + 1;
    b = b + @$""!"";
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 1;
    var b = @$""hello"";
    foo(ref a, ref b);
    printLine(a);
    printLine(b);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalValue 1))))) (varDef var (assignableValue b) = (expression (value (literal (literalDataStructure ""hello""))))) (callStatement (expression (methodCall foo ( (argumentList (expression (value a)) , (expression (value b))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value b))) ))))) end main) (procedureDef procedure (procedureSignature foo ( (parameterList (parameter a (type Int)) , (parameter b (type String))) )) (statementBlock (assignment (assignableValue a) = (expression (expression (value a)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1)))))) (assignment (assignableValue b) = (expression (expression (value b)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalDataStructure ""!""))))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello!\r\n");
    }

    [TestMethod]
    public void Pass_NestedCalls() {
        var code = @"
main
    foo()
    printLine(3)
end main

procedure foo()
    printLine(1)
    bar()
end procedure

procedure bar()
    printLine(2)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    printLine(1);
    bar();
  }
  public static void bar() {
    printLine(2);
  }
}

public static class Program {
  private static void Main(string[] args) {
    foo();
    printLine(3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall bar ( ))))) end procedure) (procedureDef procedure (procedureSignature bar ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n2\r\n3\r\n");
    }

    [TestMethod]
    public void Pass_Recursion() {
        var code = @"
main
    foo(3)
end main

procedure foo(a Int)
    if a > 0 then
        printLine(a)
        foo(a-1)
    end if
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a) {
    if (a > 0) {
      printLine(a);
      var _foo_0 = a - 1;
      foo(ref _foo_0);
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var _foo_0 = 3;
    foo(ref _foo_0);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall foo ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( (parameterList (parameter a (type Int))) )) (statementBlock (proceduralControlFlow (if if (expression (expression (value a)) (binaryOp (conditionalOp >)) (expression (value (literal (literalValue 0))))) then (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) )))) (callStatement (expression (methodCall foo ( (argumentList (expression (expression (value a)) (binaryOp (arithmeticOp -)) (expression (value (literal (literalValue 1)))))) ))))) end if))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n2\r\n1\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_CallingUndeclaredProc() {
        var code = @"
main
    bar()
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_TypeSpecifiedBeforeParamName() {
        var code = @"
main
end main

procedure foo(Int a) 
    printLine(a)
end procedure
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoEnd() {
        var code = @"
main
    printLine(1)
    foo()
    printLine(3)
end main

procedure foo()
    printLine(2)
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CannotCallMain() {
        var code = @"
main
    printLine(1)
    foo()
    printLine(3)
end main

procedure foo()
    main()
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_PassingUnnecessaryParameter() {
        var code = @"
main
    printLine(1)
    foo(3)
    printLine(3)
end main

procedure foo()
    printLine(2)
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_PassingTooFewParams() {
        var code = @"
main
    var a = 1
    foo(a + 1)
end main

procedure foo (a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_PassingWrongType() {
        var code = @"
main
    foo(1,2)
end main

procedure foo (a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InclusionOfRefInCall() {
        var code = @"
main
    foo(ref 1,2)
end main

procedure foo(a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InclusionOfRefInDefinition() {
        var code = @"
main
    foo(byref 1,2)
end main

procedure foo(ref a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_UnterminatedRecursion() {
        var code = @"
main
    foo(3)
end main

procedure foo(a Int)
    foo(a)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a) {
    foo(ref a);
  }
}

public static class Program {
  private static void Main(string[] args) {
    var _foo_0 = 3;
    foo(ref _foo_0);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Stack overflow.");
    }

    #endregion
}