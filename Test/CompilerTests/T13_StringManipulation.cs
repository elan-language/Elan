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
    print a + "" ""+ b
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""Hello"";
    var b = @$""World!"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(a + @$"" "" + b));
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
    print '_'+""Hello""+'!'
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString('_' + @$""Hello"" + '!'));
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
    print ""Hello""+3.1
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""Hello"" + 3.1));
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
    print a[2]
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[2]));
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
    print a[1..3]
    print a[2..]
    print a[..2]
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[(1)..(3)]));
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[(2)..]));
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[..(2)]));
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
    print ""abc"" is ""abc""
    print ""abc"" is ""abcd""
    print ""abc"" is ""Abc""
    print ""abc"" is ""abc""
    print ""abc"" is not ""abcd""
    print ""abc"" is not ""abcd""
    print ""abc"" is not ""Abc""
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" == @$""abc""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" == @$""abcd""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" == @$""Abc""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" == @$""abc""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" != @$""abcd""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" != @$""abcd""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" != @$""Abc""));
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
    print ""abc"".isBefore(""abC"")
    print ""abcd"".isAfter(""abc"")
    print ""abc"".isAfterOrSameAs(""abc"")
    print ""abc"".isBeforeOrSameAs(""abc"")
    print ""abcd"".isAfterOrSameAs(""abc"")
    print ""abcd"".isBeforeOrSameAs(""abc"")
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.isBefore(@$""abc"", @$""abC"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.isAfter(@$""abcd"", @$""abc"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.isAfterOrSameAs(@$""abc"", @$""abc"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.isBeforeOrSameAs(@$""abc"", @$""abc"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.isAfterOrSameAs(@$""abcd"", @$""abc"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.isBeforeOrSameAs(@$""abcd"", @$""abc"")));
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
    print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    a = StandardLibrary.Functions.asString((2.1 + 3.4));
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5.5\r\n");
    }

    [TestMethod]
    public void Pass_Interpolation() {
        var code = @"
main
    var a = 3
    var b = 4
    var c = ""{a} x {b} = {a * b}""
    print c
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    var b = 4;
    var c = @$""{a} x {b} = {a * b}"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
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
    print c
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    var b = 4;
    var c = @$""{{{a} x {b}}} = {a * b}"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
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
    print c
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var c = @$""Hello
 World!"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
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
    print c
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var c = @$""Hello "" + newline + @$""World!"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
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
    print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3.1 + @$""Hello"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
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
    print a[5]
end main
";
        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[5]));
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
    print ""abc"" < ""abC""
    print ""abcd"" > ""abc""
    print ""abc"" >= ""abc""
    print ""abc"" <= ""abc""
    print ""abcd"" >= ""abc""
    print ""abcd"" <= ""abc""
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" < @$""abC""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abcd"" > @$""abc""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" >= @$""abc""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abc"" <= @$""abc""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abcd"" >= @$""abc""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""abcd"" <= @$""abc""));
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
    print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    a = 2.1 + 3.4;
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
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