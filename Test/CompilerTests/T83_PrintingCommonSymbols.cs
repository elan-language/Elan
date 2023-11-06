using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T83_PrintingCommonSymbols
{
    #region Passes


    [TestMethod, Ignore]
    public void Pass_CommonSymbolsAccessibleFromUKKeyboard()
    {
        var code = @"#
main
  printLine(""¬!£$%^&*()@~#`|<>'€"")
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(@$""¬!£$%^&*()@~#`|<>'€"");
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "¬!£$%^&*()@~#`|<>'€\r\n");
    }

    #endregion

    #region Fails

    #endregion
}