using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T96_Snake {
    #region Passes

    [TestMethod]
    public void Pass_ConsoleUI() {
        var code = ReadElanSourceCodeFile("snake.elan");

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
    }

    #endregion

    #region Fails

    #endregion
}