using Compiler;

namespace Test.CompilerTests;

using static Helpers;


[TestClass, Ignore]
public class T10_WhileLoop
{
    [TestMethod]
    public void Pass_minimal()
    {
        var code = @"
main
   var x = 0
   while x < 10
     x = x + 1
   end while
   printLine(x)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
   var x = 0;
   while (x < 10) {
        x = x + 1;
   }
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
    public void Fail_noEnd()
    {
        var code = @"
main
   var x = 0
   while x < 10
     x = x + 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_variableNotPredefined()
    {
        var code = @"
main
   while x < 10
     x = x + 1
   end while
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_variableDefinedInWhile()
    {
        var code = @"
main
   while var x < 10
     x = x + 1
   end while
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_noCondition()
    {
        var code = @"
main
   var x = 0
   while
     x = x + 1
   end while
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

}