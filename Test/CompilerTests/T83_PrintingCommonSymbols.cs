using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T83_PrintingCommonSymbols
{
    #region Passes

    // This test is actually just testing the test code runner. On my machine € doesn't work - but it also doesn't display 
    // in a cmd prompt - so may be just locale issue. 
    [TestMethod]
    public void Pass_CommonSymbolsAccessibleFromUKKeyboard()
    {
        var code = @"#
main
  print ""¬!£$%^&*()@~#`|<>'""
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""¬!£$%^&*()@~#`|<>'""));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "¬!£$%^&*()@~#`|<>'\r\n");
    }

    #endregion

    #region Fails

    #endregion
}