using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T73_Lambdas {
    #region Passes

    [TestMethod]
    public void Pass_PassAsParam()
    {
        var code = @"
main
  printModified(4, lambda x -> x * 3)
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_ReturnALambda()
    {
        var code = @"
main
  print getFunc()(7)
end main

function getFunc() as (Int -> Int)
  return lambda x -> x * 5
end function

";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "35\r\n");
    }

    [TestMethod]
    public void Pass_ReturnALambdaWithDifferentType()
    {
        var code = @"
main
  print getFunc()(3)
end main

function getFunc() as (Int -> String)
  return lambda x -> ""abcde""[..x]
end function

";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "abc\r\n");
    }


    #endregion

    #region Fails
    [TestMethod]
    public void Fail_PassLambdaWithWrongTypes()
    {
        var code = @"
main
  printModified(4, lambda x -> x.asString())
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }

    [TestMethod]
    public void Fail_InvokeLambdaWithWrongType()
    {
        var code = @"
main
  printModified('4', lambda x -> x * 3)
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }
    #endregion
}