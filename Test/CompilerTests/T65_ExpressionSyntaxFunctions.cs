using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T65_ExpressionSyntaxFunctions {
    #region Fails

    [TestMethod]
    public void Fail_endFunction() {
        var code = @"# Elanv0.1 Parsed FFFF
main
 print square(3)
end main

function square(x Int) as Int -> x * x
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion

    #region Passes

    [TestMethod]
    public void Pass_Simple() {
        var code = @"# Elanv0.1 Parsed FFFF
main
 print square(3)
end main

function square(x Int) as Int -> x * x
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.square(3)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "9\r\n");
    }

    [TestMethod]
    public void Pass_NoParams() {
        var code = @"# Elanv0.1 Parsed FFFF
main
 print phi()
end main

function phi() as Float -> (1 + sqrt(5))/2
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static double phi() {

    return Compiler.WrapperFunctions.FloatDiv((1 + StandardLibrary.Functions.sqrt(5)), 2);
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.phi()));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1.618033988749895\r\n");
    }

    #endregion
}