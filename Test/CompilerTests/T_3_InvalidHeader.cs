using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_3_Header {
    #region Passes

    [TestMethod]
    public void Pass_header() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
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
        var code = @"# eb3dc7628a465e1405e80ea4a9a217a22f05234a9fa95484313cd353128c81aa Elan v0.1 valid

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
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.2 valid
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Incorrect Elan version expect: 'v0.1'");
    }

    [TestMethod]
    public void Fail_BadLanguage() {
        var code = @"# FFFFFFFFFFFFFFFF Python v0.1 valid
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Incorrect language expect: 'Elan'");
    }


    [TestMethod]
    public void Fail_BadStatus() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 Incomplete
main
  print ""Hello World!""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));

        AssertInvalidHeader(compileData, "Status must be 'valid'");
    }

    [TestMethod]
    public void Fail_BadHash() {
        var code = @"# eb3dc7628a465e15 Elan v0.1 valid

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