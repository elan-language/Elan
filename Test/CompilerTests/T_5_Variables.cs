using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T_5_Variables
    {

    [TestMethod]
    public void Pass_Int()
    {
        var code = @"
main
  var a = 3
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
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_Reassign()
    {
        var code = @"
main
  var a = 3
  a = 4
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
    public void Pass_CoerceFloatToIntVar()
    {
        var code = @"
main
  var a = 3.1
  a = 4
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
    public void fail_wrongKeyword()
    {
        var code = @"
main
  variable a = 3
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_duplicateVar()
    {
        var code = @"
main
  var a = 3
  var a = 4
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        Assert.Fail("Should fail compilation");
    }

    [TestMethod]
    public void fail_duplicateVarConst()
    {
        var code = @"
main
  const a = 3
  var a = 4
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        Assert.Fail("Should fail compilation");
    }

    [TestMethod]
    public void fail_GlobalVariable()
    {
        var code = @"
var a = 4
main
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_assignIncompatibleType()
    {
        var code = @"
main
  var a = 4
  a = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        Assert.Fail("Should fail compilation");
    }

    [TestMethod]
    public void fail_notInitialised()
    {
        var code = @"
main
  var a
  a = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

}

