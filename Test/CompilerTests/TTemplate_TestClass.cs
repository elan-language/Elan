using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class Template_TestClass {
    #region Passes

    [TestMethod]
    public void Pass_Template() {
        var code = @"
main
 print 
end main
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "\r\n");
    }

    #endregion

    #region Fails

    #endregion
}