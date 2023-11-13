using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T62_Tuples {
    #region Passes

    [TestMethod]
    public void Pass_CreatingTuplesAndReadingContents() {
        var code = @"#
main
    var x = (3,""Apple"")
    call printLine(x)
    call printLine(x[0])
    call printLine(x[1])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = (3, @$""Apple"");
    printLine(x);
    printLine(x.Item1);
    printLine(x.Item2);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(3, Apple)\r\n3\r\nApple\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_DeconstructIntoExistingVariables() {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 0
    var z = """"
    set (y, z) to x
    call printLine(y)
    call printLine(z)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = (3, @$""Apple"");
    var y = 0;
    var z = @$"""";
    (y, z) = x;
    printLine(y);
    printLine(z);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_DeconstructIntoNewVariables() {
        var code = @"#
main
    var x = (3,""Apple"")
    (var y, var z) = x
    call printLine(y)
    call printLine(z)
end main
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = (3, @$""Apple"");
    (var y, var z) = x;
    printLine(y);
    printLine(z);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfSameType() {
        var code = @"#
main
    var x = (3,""Apple"")
    set x to (4,""Pear"")
    call printLine(x)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = (3, @$""Apple"");
    x = (4, @$""Pear"");
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
        AssertObjectCodeExecutes(compileData, "(4, Pear)\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_OutOfRangeError() {
        var code = @"#
main
    var x = (3,""Apple"")
    call printLine(x[2])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = (3, @$""Apple"");
    printLine(x.Item3);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AssignItemToWrongType() {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 4
    set y to x[1]
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
    public void Fail_ImmutableSoCannotAssignAnItem() {
        var code = @"#
main
    var x = (3,""Apple"")
    set x[0] to 4
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify an element within a tuple");
    }

    [TestMethod]
    public void Fail_DeconstructIntoWrongType() {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 0
    var z = """"
    set (z, y) to x
    call printLine(y)
    call printLine(z)
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
    public void Pass_AssignANewTupleOfWrongType() {
        var code = @"#
main
    var x = (3,""Apple"")
    set x to (""4"",""Pear"")
    call printLine(x)
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}