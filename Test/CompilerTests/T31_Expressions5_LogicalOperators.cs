using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T31_LogicalOperators {
    [TestMethod]
    public void Pass_and() {
        var code = @"#
main
    var a = false and false
    var b = false and true
    var c = true and false
    var d = true and true
    printLine(a)
    printLine(b)
    printLine(c)
    printLine(d)
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
    var a = false && false;
    var b = false && true;
    var c = true && false;
    var d = true && true;
    printLine(a);
    printLine(b);
    printLine(c);
    printLine(d);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "False\r\nFalse\r\nFalse\r\nTrue\r\n");
    }

    [TestMethod] [Ignore]
    public void Pass_or() {
        var code = @"#
main
    var a = false or false
    var b = false or true
    var c = true or false
    var d = true or true
    printLine(a)
    printLine(b)
    printLine(c)
    printLine(d)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
    var a = false || false;
    var b = false || true;
    var c = true || false;
    var d = true || true;
    printLine(a);
    printLine(b);
    printLine(c);
    printLine(d);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\ntrue\r\ntrue\r\n");
    }

    [TestMethod] [Ignore]
    public void Pass_xor() {
        var code = @"#
main
    var a = false xor false
    var b = false xor true
    var c = true xor false
    var d = true xor true
    printLine(a)
    printLine(b)
    printLine(c)
    printLine(d)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
    var a = xor(false, false);
    var b = xor(false, true);
    var c = xor(true, false);
    var d = xor(true, true);
    printLine(a);
    printLine(b);
    printLine(c);
    printLine(d);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod] [Ignore]
    public void Pass_not() {
        var code = @"#
main
    var a = not false
    var b = not true
    var c = not not true
    var d = not not false
    printLine(a)
    printLine(b)
    printLine(c)
    printLine(d)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
    var a = !false;
    var b = !true;
    var c = !!false;
    var d = !!true;
    printLine(a);
    printLine(b);
    printLine(c);
    printLine(d);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod] [Ignore]
    public void Pass_Precedence() {
        var code = @"#
main
    var a = not false and true
    var b = not (false and true)
    printLine(a)
    printLine(b)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
    var a = !false && true;
    var b = !(false && true);
    printLine(a);
    printLine(b);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    [TestMethod] [Ignore]
    public void Fail_UseNotWithTwoArgs() {
        var code = @"#
    main
      var a = true not false
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}