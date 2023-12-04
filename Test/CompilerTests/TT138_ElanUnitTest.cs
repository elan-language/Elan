using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class TT138_ElanUnitTest
{
    #region Passes

    [TestMethod]
    public void Pass_Template() {
        var code = @"
test square_happyCase1
  assert square(3) is 9
end test

function square(x Int) as Int -> x * x
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

[TestClass}
public partial class Tests {

  [TestMethod]
  public void square_happyCase1() {
  {
    Assert.AreEqual(9,square(3)) 
  }
}

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
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