using Compiler;
namespace Test.CompilerTests;
using static Helpers;

[TestClass]
public class T38_ExpressionsSplitOverMoreThanOneLine
{
    #region Passes
    [TestMethod]
    public void Pass_1()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 +
     cos(x) ^ 2
  printLine(y)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0.7;
    var y = System.Math.Pow(sin(x), 2) + System.Math.Pow(cos(x), 2);
    printLine(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_2()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 
       2 + cos(x) ^ 2
  printLine(y)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0.7;
    var y = System.Math.Pow(sin(x), 2) + System.Math.Pow(cos(x), 2);
    printLine(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_3()
    {
        var code = @"#
main
  var x = 0.7
  var y = 
sin(x) ^ 2 + cos(x) ^ 2
  printLine(y)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0.7;
    var y = System.Math.Pow(sin(x), 2) + System.Math.Pow(cos(x), 2);
    printLine(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }
    #endregion

    #region Fails
    [TestMethod]
    public void Fail_1()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 
     + cos(x) ^ 2
  printLine(y)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_2()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(
        x) ^ 2  + cos(x) ^ 2
  printLine(y)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_3()
    {
        var code = @"#
main
  var x = 0.7
  var y 
     = sin(x) ^ 2  + cos(x) ^ 2
  printLine(y)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
    #endregion
}