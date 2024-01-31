using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_3_InvalidHeader {
    [TestMethod]
    public void Fail_noComment() {
        var code = @"main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
       
        AssertInvalidHeader(compileData, "Header must be comment with Elan, version, status and hash");
    }

    [TestMethod]
    public void Fail_noContent() {
        var code = @"#
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
       
        AssertInvalidHeader(compileData, "Header must be comment with Elan, version, status and hash");
    }

    [TestMethod]
    public void Fail_BadStatus() {
        var code = @"# Elan v0.1 Incomplete FFFFFFFFFFFFFFFF
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
       
        AssertInvalidHeader(compileData, "Status must be 'valid'");
    }

    [TestMethod]
    public void Fail_BadHash() {
        var code = @"# Elan v0.1 valid AAAAAAAAAAAAAAAA

main
  # My first program
  print ""Hello World!"" 
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
       
        AssertInvalidHeader(compileData, "Hash check failed");
    }

    [TestMethod]
    public void Pass_header() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
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

    [TestMethod]
    public void Pass_hash() {
        var code = @"# Elan v0.1 valid 5500d24de64ecce5

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