using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class TT138_ElanUnitTest {
    #region Passes

    [TestMethod]
    public void Pass_1() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
test squareHappyCase1
    assert square(3) is 9
end test

function square(x Int) as Int
    return x * x
end function

main
    var a set to square(3)
    print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int square(int x) {

    return x * x;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = Globals.square(3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}

[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
public class _Tests {
  [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
  public void squareHappyCase1() {
    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(9, Globals.square(3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "9\r\n");
    }

    #endregion

    #region Fails

    #endregion
}