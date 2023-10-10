using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T17_Dictionaries
{
    #region Passes

    [TestMethod]
    public void Pass_LiteralConstant() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a)
  printLine(a['z'])
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
        AssertObjectCodeExecutes(compileData, "{'a':1, 'b':3, 'z':10}\r\n10\r\n");
    }

    [TestMethod]
    public void Pass_ChangeEntry()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  a['b'] = 4
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
        AssertObjectCodeExecutes(compileData, "{'a':1, 'b':4, 'z':10}\r\n");
    }

    //TODO
    // Add new item - testing that target is not mutated
    // count
    // contains(key)
    #endregion

    #region Fails
    [TestMethod]
    public void Fail_RepeatedKey()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'a':10}
main
  printLine(a)
end main
";

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    //TODO
    //key not present
    //invalid type for key or value


    #endregion
}