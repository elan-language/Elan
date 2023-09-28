using Compiler;
using static Antlr4.Runtime.Atn.SemanticContext;
using System.Runtime.Intrinsics.X86;

namespace Test.CompilerTests;

using static Antlr4.Runtime.Atn.SemanticContext;
using static Helpers;

[TestClass, Ignore]
public class T15_ForInLoop
{
    #region Passes
    [TestMethod]
    public void Pass_List()
    {
        var code = @"
main
    var a = {7,8,9}
    var n = 0
    for x in a
        n = n + x
    end for
    printLine(n)
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
    var a = new List<int> {7,8,9};
    var n = 0;
    foreach (var x in a) {
        n = n + x;
    }
    printLine(n);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "24\r\n");
    }

    [TestMethod]
    public void Pass_string()
    {
        var code = @"
main
    var a = ""hello""
    for x in a
        printLine(x)
    end for
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
    var a = ""hello"";
    foreach (var x in a) {
        printLine(x);
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "h\r\ne\r\nl\r\nl\r\no\r\n");
    }

    [TestMethod]
    public void Pass_doubleLoop()
    {
        var code = @"
main
    for x in ""12""
        for y in ""34""
            printLine(""{x}{y }"")
        end for
    end for
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
    foreach (var x in ""12"") {
        foreach (var y in ""34"") {
            printLine(""{x}{y }"");
        }
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "13\r\n14\r\n23\r\n24\r\n");
    }

    [TestMethod]
    public void Pass_variableIsScoped()
    {
        var code = @"
main
    var a = {7,8,9}
    var x = ""hello"";
    for x in a
       printLine(n)
    end for
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
    var a = new List<int> {7,8,9};
    var x = ""hello"";
    for x in a
       printLine(x)
    end for
    printLine(x)
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n8\r\n9\r\nhello\r\n");
    }
    #endregion

    #region Fails
    [TestMethod]
    public void Fail_NoEndFor()
    {
        var code = @"
main
  var a = ""hello""
  for x in a
   printLine(x)
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_refereranceVariableOutsideLoop()
    {
        var code = @"
main
    var a = {7,8,9}
    for x in a
       printLine(n)
    end for
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
    var a = new List<int> {7,8,9};
    for x in a
       printLine(x)
    end for
    printLine(x)
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_applyToANonIterable()
    {
        var code = @"
main
    for x in 10
       printLine(n)
    end for
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_CannotAlterEnumerableWithinLoop()
    {
        var code = @"
main
  var a = new Array {1,2,3,4,5}
  for x in a
    a[x] = 0
  end for
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    #endregion
}