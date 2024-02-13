using Compiler;
using Test.CompilerTests;

namespace Test.ExampleProjects;

using static Helpers;

[TestClass]
public class Snake_test
{
    #region Passes

    [TestMethod]
    public void Pass_Snake_OOP()
    {
        var code = ReadCodeFile("snake_OOP.elan");

        var objectCode = ReadCodeFile("snake_OOP.obj");

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }

    [TestMethod]
    public void Pass_Snake_PP()
    {
        var code = ReadCodeFile("snake_PP.elan");

        var objectCode = ReadCodeFile("snake_PP.obj");

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }
    #endregion

    #region Fails

    #endregion
}