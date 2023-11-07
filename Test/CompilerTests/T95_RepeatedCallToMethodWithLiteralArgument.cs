using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T95_RepeatedCallToProcedureWithLiteralArgument
{
    #region Passes

    [TestMethod]
    public void Pass_Template() {
        var code = @"
main
  square(3)
  square(5)
end main

procedure square(x Int)
  printLine(x * x)
end procedure
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "9\r\n25\r\n");
    }

    #endregion

    #region Fails

    #endregion
}