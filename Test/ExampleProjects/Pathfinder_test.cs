using Compiler;
using Test.CompilerTests;

namespace Test.ExampleProjects;

using static Helpers;

[TestClass]
public class Pathfinder_test
{
    #region Passes

    [TestMethod]
    public void Pass_Pathfinder()
    {
        var code = ReadCodeFile("Pathfinder.elan");
        var objectCode = ReadCodeFile("Pathfinder.obj");

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        ///AssertObjectCodeExecutes(compileData, "\r\n");
    }

    #endregion

    #region Fails

    #endregion
}