using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T30_Expressions4_SystemCalls
{
    [TestMethod]
    public void Pass_LibraryConst() {
        var code = @"#
main
  var a = input(""Your name"")
  printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = input(""Your name"");
    printLine(a);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Your nameFred\r\n", "Fred"); 
    }

    [TestMethod]
    public void Pass_SingleFunctionCall()
    {
        var code = @"#
main
  var x = tan(pi/4)
  printLine(x)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var x = tan(Compiler.WrapperFunctions.FloatDiv(pi, 4));
    printLine(x);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0.9999999999999999\r\n");
    }

    [TestMethod]
    public void Pass_MoreComplexExpression()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 + cos(x) ^ 2
  printLine(x)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0.7;
    var y = System.Math.Pow(sin(x), 2) + System.Math.Pow(cos(x), 2);
    printLine(x);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0.7\r\n"); //Add full digits
    }


    [TestMethod]
    public void Fail_StandaloneFunctionCallNotValid()
    {
        var code = @"#
    main
      sin(1)
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData);
    }

}