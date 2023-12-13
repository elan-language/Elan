using Compiler;
using Test.CompilerTests;

namespace Test.ExampleProjects;

using static Helpers;

[TestClass]
public class Life_test
{
    #region Passes

    [TestMethod]
    public void Pass_Life()
    {
        var code = ReadCodeFile("Life.elan");
        var objectCode = ReadCodeFile("Life.obj");

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