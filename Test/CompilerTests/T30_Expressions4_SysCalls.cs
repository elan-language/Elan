using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T30_Expressions4_SystemCalls
{
    [TestMethod]
    public void Pass_Input1()
    {
        var code = @"#
main
  var a = input()
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
    var a = input();
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
        AssertObjectCodeExecutes(compileData, "Felicity\r\n", "Felicity");
    }

    [TestMethod]
    public void Pass_Input2() {
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
    public void Fail_UnconsumedResultFromSystemCall()
    {
        var code = @"#
main
  input(""Your name"")
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_SystemCallWithinExpression()
    {
        var code = @"#
main
  var a = ""Hello"" + input(""Your name"");
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SystemCallWithinExpression2()
    {
        var code = @"#
main
  var a = input(""Your name"").ToUpper();
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }


}