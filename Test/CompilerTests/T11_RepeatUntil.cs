using Compiler;

namespace Test.CompilerTests;

using static Helpers;


[TestClass, Ignore]
public class T11_RepeatUntil
{
    [TestMethod]
    public void Pass_minimal()
    {
        var code = @"
main
   var x = 0
   repeat
     x = x + 1
   until  x >= 10
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
   var x = 0;
   do {
        x = x + 1;
   } while (!(x >= 10));
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
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_innerLoop()
    {
        var code = @"
main
   var t = 0
   var x = 0
   repeat
    var y = 0
       repeat
         y = y + 1
         t = t + 1
       until  y > 4
     x = x + 1
   until  x > 3
   printLine(t)
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
        var x = 0;
        do {
            var y = 0;
            do {
                t = t + 1
                y = y + 1;
            } while (!(y > 4));
            x = x + 1;
        } while (!(x > 2));
        printLine(t);
    }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "8\r\n");
    }

    [TestMethod]
    public void Fail_noUntil()
    {
        var code = @"
main
   var x = 0
   repeat
     x = x + 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_variableRedeclaredInTest()
    {
        var code = @"
main
    var x = 0
    repeat
      x = x + 1
   until var x >= 10
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_variableDefinedInLoop()
    {
        var code = @"
main
   repeat
     var x = x + 1
   until  x >= 10
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }


    [TestMethod]
    public void Fail_testPutOnRepeat()
    {
        var code = @"
main
    var x = 0
    repeat x >= 10
      x = x + 1
    until 
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

 
    [TestMethod]
    public void Fail_noCondition()
    {
        var code = @"
main
    var x = 0
    repeat
      x = x + 1
    until 
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_invalidCondition()
    {
        var code = @"
main
    var x = 0
    repeat
      x = x + 1
    until >= 10
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

}