using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T20_StatementBodiedFunctions {
    #region Passes

    [TestMethod]
    public void Pass_SimpleCase() {
        var code = @"
main
    printLine(foo(3,4))
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int foo(int a, int b) {

    return a * b;
  }
}

public static class Program {
  private static void Main(string[] args) {
    printLine(foo(3, 4));
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (methodCall foo ( (argumentList (expression (value (literal (literalValue 3)))) , (expression (value (literal (literalValue 4))))) )))) ))))) end main) (functionDef (functionWithBody function (functionSignature foo ( (parameterList (parameter a (type Int)) , (parameter b (type Int))) ) as (type Int)) statementBlock return (expression (expression (value a)) (binaryOp (arithmeticOp *)) (expression (value b))) end function)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_Recursive() {
        var code = @"
main
    printLine(factorial(5))
end main

function factorial(a Int) as Int
    var result = 0;
    if a > 2 then
        result = a * factorial(a-1)
    else 
        result = a
    end if
    return result
end function
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int factorial(int a) {
    var result = 0;
    if (a > 2) {
      result = a * factorial(a - 1);
    }
    else {
      result = a;
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {
    printLine(factorial(5));
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (methodCall factorial ( (argumentList (expression (value (literal (literalValue 5))))) )))) ))))) end main) (functionDef (functionWithBody function (functionSignature factorial ( (parameterList (parameter a (type Int))) ) as (type Int)) (statementBlock (varDef var (assignableValue result) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (if if (expression (expression (value a)) (binaryOp (conditionalOp >)) (expression (value (literal (literalValue 2))))) then (statementBlock (assignment (assignableValue result) = (expression (expression (value a)) (binaryOp (arithmeticOp *)) (expression (methodCall factorial ( (argumentList (expression (expression (value a)) (binaryOp (arithmeticOp -)) (expression (value (literal (literalValue 1)))))) )))))) else (statementBlock (assignment (assignableValue result) = (expression (value a)))) end if))) return (expression (value result)) end function)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "120\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_noEnd() {
        var code = @"
main
    foo(3,4)
end main

function foo(a Int, b Int) as Int
    return a * b
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noReturnType() {
        var code = @"
main
end main

function foo(a Int, b Int)
    return a * b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noAs() {
        var code = @"
main
end main

function foo(a Int, b Int) Int
    return a * b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noReturn() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    var c = a * b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_returnTypeIncompatible() {
        var code = @"
main
   var a = """"
   a = foo(3,4)
end main

function foo(a Int, b Int) as Int
    var c = a * b
    return c
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_noReturn2() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    var c = a * b
    return
end function
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_embeddedReturns() {
        var code = @"
main
end main

function foo(a Int, b Int) as Bool
    if 2 > 1 then
        return true
    else
        return false
    end if
end function
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_nonMatchingReturn2() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    return a / b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_statementAfterReturn() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    return a * b
    var c = a + b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CanNotContainSystemCalls() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    print(a)
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_CanNotContainProcedureCall() {
        var code = @"
main
    var result = foo(3,4)
    printLine(result)
end main

function foo(a Int, b Int) as Int
    bar()
    return a * b
end function

procedure bar() 

end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_CannotModifyParam() {
        var code = @"
main
    var result = foo(3,4)
    printLine(result)
end main

function foo(a Int, b Int) as Int
    a = a + 1
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_TooManyParams() {
        var code = @"
main
    var result = foo(3,4,5)
    printLine(result)
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NotEnoughParams() {
        var code = @"
main
    var result = foo(3)
    printLine(result)
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_WrongParamType() {
        var code = @"
main
    var result = foo(3, ""b"")
    printLine(result)
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}