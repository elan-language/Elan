using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] 
public class TT145_Life
{
    #region Passes

    [TestMethod, Ignore]
    public void Pass_Template() {
        var code = ReadElanSourceCodeFile("Life.elan");

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        ///AssertObjectCodeExecutes(compileData, "\r\n");
    }

    #endregion

    #region Fails

    #endregion
}