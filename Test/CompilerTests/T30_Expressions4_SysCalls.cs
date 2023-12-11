using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T30_Expressions4_SystemCalls {
    [TestMethod]
    public void Pass_Input1() {
        var code = @"#
main
  var a set to input
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
    var a = ((Func<string>)(() => {return Console.ReadLine() ?? """";}))();
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
  var a set to input ""Your name""
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
    var a = ((Func<string>)(() => { System.Console.Write(""Your name""); return Console.ReadLine() ?? """";}))();
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
    public void Pass_Me()
    {
        var code = @"#
main
  var a set to system.me()
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
    var a = StandardLibrary.SystemAccessors.me();
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
        var expected = Environment.UserName;
        AssertObjectCodeExecutes(compileData, $"{expected}\r\n");
    }

    [TestMethod]
    public void Fail_NoQualifier1() {
        var code = @"#
main
  var a set to input()
  print a
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoQualifier2() {
        var code = @"#
main
  var a set to input(""Your name"")
  print a
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_UnconsumedResultFromSystemCall() {
        var code = @"#
main
  call system.input(""Your name"")
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_SystemCallWithinExpression() {
        var code = @"#
main
  var a set to system.input(""Your name"").length()
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_SystemCallUsingDotSyntax() {
        var code = @"#
main
  var prompt set to ""Your name""
  var a = prompt.system.input()
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}