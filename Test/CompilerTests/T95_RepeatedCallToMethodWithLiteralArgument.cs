using Compiler;
using CSharpLanguageModel;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T95_RepeatedCallToProcedureWithLiteralArgument
{
    [TestInitialize]
    public void TestInit() {
        CodeHelpers.ResetUniqueId();
    }


    #region Passes

    [TestMethod]
    public void Pass_Template() {
        var code = @"
main
  call square(3)
  call square(5)
end main

procedure square(x Int)
  call printLine(x * x)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void square(ref int x) {
    printLine(x * x);
  }
}

public static class Program {
  private static void Main(string[] args) {
    var _square_1_0 = 3;
    square(ref _square_1_0);
    var _square_2_0 = 5;
    square(ref _square_2_0);
  }
}";

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