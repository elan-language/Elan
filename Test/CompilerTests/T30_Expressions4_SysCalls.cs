using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T30_Expressions4_SystemCalls {
    [TestMethod]
    public void Pass_Input1() {
        var code = @"#
main
  var a = system.input()
  print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = StandardLibrary.SystemAccessors.input();
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
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
  var a = system.input(""Your name"")
  print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = StandardLibrary.SystemAccessors.input(@$""Your name"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
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
    public void Fail_NoQualifier1() {
        var code = @"#
main
  var a = input()
  print a
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "");
    }

    [TestMethod]
    public void Fail_NoQualifier2() {
        var code = @"#
main
  var a = input(""Your name"")
  print a
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "");
    }

    [TestMethod]
    public void Fail_UnconsumedResultFromSystemCall() {
        var code = @"#
main
  call system.input(""Your name"")
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
      
    }

    [TestMethod]
    public void Fail_SystemCallWithinExpression() {
        var code = @"#
main
  var a = system.input(""Your name"").length()
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
      
    }

    [TestMethod]
    public void Fail_SystemCallUsingDotSyntax() {
        var code = @"#
main
  var prompt = ""Your name""
  var a = system.prompt.input()
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
   
    }
}