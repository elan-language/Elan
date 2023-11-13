using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T13_StringManipulation {
    #region Passes

    [TestMethod]
    public void Pass_AppendStrings() {
        var code = @"
main
    var a = ""Hello""
    var b = ""World!""
    printLine(a + "" ""+ b)
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
    var a = @$""Hello"";
    var b = @$""World!"";
    printLine(a + @$"" "" + b);
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
    public void Pass_AppendOrPrependChar() {
        var code = @"
main
    printLine('_'+""Hello""+'!')
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
    printLine('_' + @$""Hello"" + '!');
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "_Hello!\r\n");
    }

    [TestMethod]
    public void Pass_AppendNumber() {
        var code = @"
main
    printLine(""Hello""+3.1)
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
    printLine(@$""Hello"" + 3.1);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello3.1\r\n");
    }

    [TestMethod]
    public void Pass_Indexing() {
        var code = @"
main
    var a = ""abcde""
    printLine(a[2])
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
    var a = @$""abcde"";
    printLine(a[2]);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "c\r\n");
    }

    [TestMethod]
    public void Pass_Ranges() {
        var code = @"
main
    var a = ""abcde""
    printLine(a[1..3])
    printLine(a[2..])
    printLine(a[..2])
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
    var a = @$""abcde"";
    printLine(a[(1)..(3)]);
    printLine(a[(2)..]);
    printLine(a[..(2)]);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "bc\r\ncde\r\nab\r\n");
    }

    [TestMethod]
    public void Pass_EqualityTesting() {
        var code = @"
main
    printLine(""abc"" is ""abc"")
    printLine(""abc"" is ""abcd"")
    printLine(""abc"" is ""Abc"")
    printLine(""abc"" is ""abc"")
    printLine(""abc"" <> ""abcd"")
    printLine(""abc"" is not ""abcd"")
    printLine(""abc"" is not ""Abc"")
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
    printLine(@$""abc"" == @$""abc"");
    printLine(@$""abc"" == @$""abcd"");
    printLine(@$""abc"" == @$""Abc"");
    printLine(@$""abc"" == @$""abc"");
    printLine(@$""abc"" != @$""abcd"");
    printLine(@$""abc"" != @$""abcd"");
    printLine(@$""abc"" != @$""Abc"");
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\nfalse\r\ntrue\r\ntrue\r\ntrue\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_ComparisonMethods() {
        var code = @"
main
    printLine(""abc"".isBefore(""abC""))
    printLine(""abcd"".isAfter(""abc""))
    printLine(""abc"".isAfterOrSameAs(""abc""))
    printLine(""abc"".isBeforeOrSameAs(""abc""))
    printLine(""abcd"".isAfterOrSameAs(""abc""))
    printLine(""abcd"".isBeforeOrSameAs(""abc""))
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
    printLine(StandardLibrary.Functions.isBefore(@$""abc"", @$""abC""));
    printLine(StandardLibrary.Functions.isAfter(@$""abcd"", @$""abc""));
    printLine(StandardLibrary.Functions.isAfterOrSameAs(@$""abc"", @$""abc""));
    printLine(StandardLibrary.Functions.isBeforeOrSameAs(@$""abc"", @$""abc""));
    printLine(StandardLibrary.Functions.isAfterOrSameAs(@$""abcd"", @$""abc""));
    printLine(StandardLibrary.Functions.isBeforeOrSameAs(@$""abcd"", @$""abc""));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\ntrue\r\ntrue\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_UseAsStringExplicitly() {
        var code = @"
main
    var a = ""abcde""
    set a to (2.1 + 3.4).asString()
    print(a)
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
    var a = @$""abcde"";
    a = StandardLibrary.Functions.asString((2.1 + 3.4));
    print(a);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5.5");
    }

    [TestMethod]
    public void Pass_Interpolation() {
        var code = @"
main
    var a = 3
    var b = 4
    var c = ""{a} x {b} = {a * b}""
    printLine(c)
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
    var a = 3;
    var b = 4;
    var c = @$""{a} x {b} = {a * b}"";
    printLine(c);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3 x 4 = 12\r\n");
    }

    [TestMethod]
    public void Pass_UseBracesInString() {
        var code = @"
main
    var a = 3
    var b = 4
    var c = ""{{{a} x {b}}} = {a * b}""
    printLine(c)
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
    var a = 3;
    var b = 4;
    var c = @$""{{{a} x {b}}} = {a * b}"";
    printLine(c);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "{3 x 4} = 12\r\n");
    }

    [TestMethod]
    public void Pass_literalNewline() {
        var code = @"
main
    var c = ""Hello
 World!""
    printLine(c)
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
    var c = @$""Hello
 World!"";
    printLine(c);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello\r\n World!\r\n");
    }

    [TestMethod]
    public void Pass_newLineConstant() {
        var code = @"
main
    var c = ""Hello ""+ newline + ""World!""
    printLine(c)
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
    var c = @$""Hello "" + newline + @$""World!"";
    printLine(c);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello \r\nWorld!\r\n");
    }

    [TestMethod]
    public void Pass_AppendStringToNumber() {
        var code = @"
main
    var a = 3.1 + ""Hello""
    printLine(a)
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
    var a = 3.1 + @$""Hello"";
    printLine(a);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3.1Hello\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_IndexOutOfRange() {
        var code = @"
main
    var a = ""abcde""
    printLine(a[5])
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
    var a = @$""abcde"";
    printLine(a[5]);
  }
}";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Index was outside the bounds of the array.");
    }

    [TestMethod]
    public void Fail_ComparisonOperators() {
        var code = @"
main
    printLine(""abc"" < ""abC"")
    printLine(""abcd"" > ""abc"")
    printLine(""abc"" >= ""abc"")
    printLine(""abc"" <= ""abc"")
    printLine(""abcd"" >= ""abc"")
    printLine(""abcd"" <= ""abc"")
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
    printLine(@$""abc"" < @$""abC"");
    printLine(@$""abcd"" > @$""abc"");
    printLine(@$""abc"" >= @$""abc"");
    printLine(@$""abc"" <= @$""abc"");
    printLine(@$""abcd"" >= @$""abc"");
    printLine(@$""abcd"" <= @$""abc"");
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
    public void Fail_CoerceNumberToString() {
        var code = @"
main
    var a = ""abcde""
    set a to 2.1 + 3.4
    print(a)
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
    var a = @$""abcde"";
    a = 2.1 + 3.4;
    print(a);
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

    #endregion
}