using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T30_Expressions4_SystemCalls {
    [TestMethod]
    public void Pass_Input1() {
        var code = @"#
main
  var a = input()
  print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = input();
    print(a);
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
  print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = input(@$""Your name"");
    print(a);
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
    public void Fail_UnconsumedResultFromSystemCall() {
        var code = @"#
main
  call input(""Your name"")
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "System call generates a result that is neither assigned nor returned");
    }

    [TestMethod]
    public void Fail_SystemCallWithinExpression() {
        var code = @"#
main
  var a = input(""Your name"").length()
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot use a system call in an expression");
    }

    [TestMethod]
    public void Fail_SystemCallUsingDotSyntax() {
        var code = @"#
main
  var prompt = ""Your name""
  var a = prompt.input()
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot use a system call in an expression ");
    }
}