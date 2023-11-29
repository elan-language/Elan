using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T10_WhileLoop {
    [TestMethod]
    public void Pass_minimal() {
        var code = @"
main
   var x set to 0
   while x < 10
     set x to x + 1
   end while
   print x
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
    var x = 0;
    while (x < 10) {
      x = x + 1;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_innerLoop() {
        var code = @"
main
    var t set to 0
    var x set to 0
    while x < 3
        var y set to 0
        while y < 4
            set y to y + 1
            set t to t + 1
        end while
        set x to x + 1
    end while
   print t
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
    var t = 0;
    var x = 0;
    while (x < 3) {
      var y = 0;
      while (y < 4) {
        y = y + 1;
        t = t + 1;
      }
      x = x + 1;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(t));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Fail_noEnd() {
        var code = @"
main
   var x = 0
   while x < 10
     set x to x + 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_variableNotPredefined() {
        var code = @"
main
   while x < 10
     set x to x + 1
   end while
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_variableDefinedInWhile() {
        var code = @"
main
   while var x < 10
     set x to x + 1
   end while
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noCondition() {
        var code = @"
main
   var x = 0
   while
     set x to x + 1
   end while
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_while_do() {
        var code = @"
main
   var x = 0
   while x < 10
     set x to x + 1
   do
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}