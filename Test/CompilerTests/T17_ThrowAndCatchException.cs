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
    throwException(""Foo"")
end main
";

        var objectCode = @"";

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
  throwException(""Foo"")
end procedure
";

        var objectCode = @"";

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
  throwException(""Foo"")
end function
";

        var objectCode = @"";

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
  catch e
    printLine(""caught"")
  end try
end main

procedure foo()
  throwException(""Foo"")
end procedure
";

        var objectCode = @"";

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
    public void Pass_CatchSystemGeneratedException()
    {
        var code = @"
main
  try
     var x = 1
     var y = 0
     var z = x / y
     printLine(""not caught"")
  catch e
    printLine(e)
  end try
end main
";

        var objectCode = @"";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 1))))) )))) (callStatement (expression (methodCall foo ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 3))))) ))))) end main) (procedureDef procedure (procedureSignature foo ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalValue 2))))) ))))) end procedure) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "?\r\n"); //Some indication of division by zero
    }

    #endregion

    #region Fails
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