using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore] 
public class T81_CompletePrograms
{
    #region Passes

    [TestMethod]
    public void Pass_Template() {
         var code = ReadInCodeFile("Wordle.elan");

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        
    }

    #endregion

    #region Fails

    #endregion
}