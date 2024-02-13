using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_3_Header {
    #region Passes

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

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }

    [TestMethod]
    public void Pass_hash() {
        var code = @"# Elan v0.1 valid eb3dc7628a465e14

main
  # My first program
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

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_noComment() {
        var code = @"main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Header must be comment with Elan, version, status and hash");
    }

    [TestMethod]
    public void Fail_noContent() {
        var code = @"#
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Header must be comment with Elan, version, status and hash");
    }

    [TestMethod]
    public void Fail_BadVersion() {
        var code = @"# Elan v0.2 valid FFFFFFFFFFFFFFFF
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Incorrect Elan version expect: 'v0.1'");
    }

    [TestMethod]
    public void Fail_BadLanguage() {
        var code = @"# Python v0.1 valid FFFFFFFFFFFFFFFF
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Incorrect language expect: 'Elan'");
    }


    [TestMethod]
    public void Fail_BadStatus() {
        var code = @"# Elan v0.1 Incomplete FFFFFFFFFFFFFFFF
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Status must be 'valid'");
    }

    [TestMethod]
    public void Fail_BadHash() {
        var code = @"# Elan v0.1 valid eb3dc7628a465e15

main
  # My first program
  print ""Hello World!"" 
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Hash check failed");
    }

    #endregion
}