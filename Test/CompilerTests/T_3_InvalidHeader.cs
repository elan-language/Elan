using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_3_InvalidHeader {
    [TestMethod]
    public void NoComment() {
        var code = @"main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
       
        AssertInvalidHeader(compileData, "Header must be comment with Elan version, status and hash");
    }

    [TestMethod]
    public void NoContent() {
        var code = @"#
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
       
        AssertInvalidHeader(compileData, "Header must be comment with Elan version, status and hash");
    }

    [TestMethod]
    public void BadStatus() {
        var code = @"# Elanv0.1 Incomplete FFFF
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
       
        AssertInvalidHeader(compileData, "Status must be 'Parsed'");
    }

    [TestMethod]
    public void Pass2() {
        var code = @"# Elanv0.1 Parsed FFFF
main
  print ""Hello World!""
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""Hello World!""));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }
}