using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T96_Snake
{
    #region Passes

    [TestMethod]
    public void Pass_Square() {
        var code = ReadElanSourceCodeFile("snake_Square.elan");

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3,4\r\ntrue\r\n3,3\r\n3,5\r\n1,4\r\n5,4\r\n");
    }

    [TestMethod]
    public void Pass_Snake()
    {
        var code = ReadElanSourceCodeFile("snake_Snake.elan");

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Snake\r\n5,4\r\n3,4\r\n7,4\r\n5,4\r\n7,5\r\n7,4\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_Game()
    {
        var code = ReadElanSourceCodeFile("snake_Game.elan");

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "\r\n");
    }

    #endregion

    #region Fails

    #endregion
}