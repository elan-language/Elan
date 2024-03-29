﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T79_InterpolatedStrings {
    #region Passes

    [TestMethod]
    public void Pass_CanUseVariables() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 1
    var b set to ""Apple""
    var c set to {1,2,3}
    print ""{a} {b} {c}""
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
    var a = 1;
    var b = @$""Apple"";
    var c = new StandardLibrary.ElanList<int>(1, 2, 3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""{a} {b} {c}""));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1 Apple List {1,2,3}\r\n");
    }

    [TestMethod]
    public void Pass_UseCSharpKeywordAsVariable() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var static set to 1
    print ""{static}""
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
    var @static = 1;
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""{@static}""));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    #endregion
}