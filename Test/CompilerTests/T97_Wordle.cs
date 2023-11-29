using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T97_Wordle {
    #region Passes

    [TestMethod, Ignore]
    public void Pass_ConsoleUI() {
        var code = ReadElanSourceCodeFile("Wordle.elan");

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
    }

    #endregion

    #region Fails

    #endregion
}