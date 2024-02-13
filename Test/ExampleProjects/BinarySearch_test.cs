using Compiler;
using Test.CompilerTests;

namespace Test.ExampleProjects;

using static Helpers;

[TestClass]
public class BinarySearch_test
{
    #region Passes

    [TestMethod]
    public void Pass_BinarySearch()
    {
        var code = ReadCodeFile("BinarySearch.elan"); ;
        var objectCode = ReadCodeFile("BinarySearch.obj");

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\ntrue\r\nfalse\r\ntrue\r\ntrue\r\nfalse\r\ntrue\r\nfalse\r\nfalse\r\n");
    }

    #endregion

    #region Fails

    #endregion
}