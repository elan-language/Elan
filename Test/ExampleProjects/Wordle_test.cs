using Compiler;
using Test.CompilerTests;

namespace Test.ExampleProjects;

using static Helpers;

[TestClass]
public class Wordle_test
{
    #region Passes

    [TestMethod]
    public void Pass_Wordle_1()
    {
        var code = ReadCodeFile("Wordle_1.elan");
        var objectCode = ReadCodeFile("Wordle_1.obj");

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }


    [TestMethod]
    public void Pass_Wordle_2()
    {
        var code = ReadCodeFile("Wordle_2.elan");
        var objectCode = ReadCodeFile("Wordle_2.obj");

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }

    #endregion

    #region Fails

    #endregion
}