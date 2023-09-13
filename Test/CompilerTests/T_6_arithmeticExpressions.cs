using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T_6_arithmeticExpressions
{
    #region Passes
    [TestMethod]
    public void Pass_IntAddition()
    {
        var code = @"
main
  printLine( 3 + 4)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    [TestMethod]
    public void Pass_IncludeVariable()
    {
        var code = @"
main
  var a = 3
  printLine( a + 4)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    [TestMethod]
    public void Pass_DivideIntegersToFloat()
    {
        var code = @"
main
  printLine(3/2)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1.5\r\n");
    }

    [TestMethod]
    public void Pass_IntegerDivision()
    {
        var code = @"
main
  printLine(7 div 2)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_Mod()
    {
        var code = @"
main
  printLine(11 mod 3)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\n");
    }

    [TestMethod]
    public void Pass_Power()
    {
        var code = @"
main
  printLine(3 ^ 3)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "81\r\n");
    }

    [TestMethod]
    public void Pass_UseVariableBothSides()
    {
        var code = @"
main
  var a = 3
  a = a + 1
  printLine(a)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    [TestMethod]
    public void Pass_TODO()
    {
        Assert.Fail("RP to write tests using standard library function calls within expressions");
    }
        #endregion
        #region Fails
        [TestMethod]
    public void Fail_InvalidExpressio()
    {
        var code = @"
main
  var a = 3 4
end main
";
    
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_PlusEquals()
    {
        var code = @"
main
  var a = 3
  a += 1
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_PlusPlus()
    {
        var code = @"
main
  var a = 3
  a ++
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
    #endregion
}