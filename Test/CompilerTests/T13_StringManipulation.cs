using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T13_StringManipulation
{
    #region Passes
    [TestMethod]
    public void Pass_AppendStrings()
    {
        var code = @"
main
    var a = ""Hello""
    var b = ""World!""
    printLine(a + "" ""+ b)
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
    var a = ""Hello"";
    var b = ""World!"";
    printLine(a + "" "" + b);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalDataStructure ""Hello""))))) (varDef var (assignableValue b) = (expression (value (literal (literalDataStructure ""World!""))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value a)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalDataStructure "" ""))))) (binaryOp (arithmeticOp +)) (expression (value b)))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }

    [TestMethod]
    public void Pass_AppendOrPrependChar()
    {
        var code = @"
main
    printLine('_'+""Hello""+'!')
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
    printLine('_' + ""Hello"" + '!');
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value (literal (literalValue '_')))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalDataStructure ""Hello""))))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue '!')))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "_Hello!\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_AppendNumber()
    {
        var code = @"
main
    printLine(""Hello""+3.1)
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
    printLine(@$""Hello ""+3.1)
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello 3.1\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_Indexing()
    {
        var code = @"
main
    var a = ""abcde""
    printLine(a[2])
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

    [TestMethod, Ignore]
    public void Pass_Ranges()
    {
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
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    printLine(a[1..3]);
    printLine(a[2..]);
    printLine(a[..2]);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "bcd\r\ncde\r\nabc\r\n");
    }


    [TestMethod]
    public void Pass_Comparison()
    {
        var code = @"
main
    printLine(""abc"" == ""abc"")
    printLine(""abc"" == ""abcd"")
    printLine(""abc"" == ""Abc"")
    printLine(""abc"" is ""abc"")
    printLine(""abc"" <> ""abcd"")
    printLine(""abc"" is not ""abcd"")
    printLine(""abc"" is not ""Abc"")
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
    printLine(""abc"" == ""abc"");
    printLine(""abc"" == ""abcd"");
    printLine(""abc"" == ""Abc"");
    printLine(""abc"" == ""abc"");
    printLine(""abc"" != ""abcd"");
    printLine(""abc"" != ""abcd"");
    printLine(""abc"" != ""Abc"");
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
    public void Fail_Comparison()
    {
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
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(""abc"" < ""abC"");
    printLine(""abcd"" > ""abc"");
    printLine(""abc"" >= ""abc"");
    printLine(""abc"" <= ""abc"");
    printLine(""abcd"" >= ""abc"");
    printLine(""abcd"" <= ""abc"");
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

    [TestMethod, Ignore]
    public void Fail_CoerceNumberToString()
    {
        var code = @"
main
    var a = ""abcde""
    a = 2.1 + 3.4
    print(a)
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
    var a = @$""abcde"";
    a = asString(2.1 + 3.4);
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
        AssertObjectCodeExecutes(compileData, "5.5\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_UseAsStringExplicitly()
    {
        var code = @"
main
    var a = ""abcde""
    a = (2.1 + 3.4).asString()
    print(a)
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
    var a = @$""abcde"";
    a = asString(2.1 + 3.4);
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
        AssertObjectCodeExecutes(compileData, "5.5\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_Interpolation()
    {
        var code = @"
main
    var a = 3
    var b = 4
    var c = ""{a} x {b} = {a * b}""
    printLine(a)
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
    var a = 3;
    var b = 4;
    var c = @$""{a} x {b} = {a * b}"";
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
        AssertObjectCodeExecutes(compileData, "3 x 4 = 12\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_UseBracesInString()
    {
        var code = @"
main
    var a = 3
    var b = 4
    var c = ""{{{a} x {b}}} = {a * b}""
    printLine(a)
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
    var a = 3;
    var b = 4;
    var c = @$""{{{a} x {b}}} = {a * b}"";
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
        AssertObjectCodeExecutes(compileData, "{3 x 4} = 12\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_literalNewline()
    {
        var code = @"
main
    var c = ""Hello
 World!""
    printLine(a)
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
    var c = $@""Hello 
World"";
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
        AssertObjectCodeExecutes(compileData, "Hello \r\nWorld!\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_newLineConstant()
    {
        var code = @"
main
    var c = ""Hello ""+ newLine + ""World!""
    printLine(a)
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
    var c = @$""Hello ""+ newLine + ""World!"";
    printLine(a);
  }
}";

        var x = @$"HelloWorld";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello \r\nWorld!\r\n");
    }

    #endregion

    #region Fails

    [TestMethod, Ignore]
    public void Fail_IndexOutOfRange()
    {
        var code = @"
main
    var a = ""abcde""
    printLine(a[5])
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
    var c = @$""abcde"";
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
        AssertObjectCodeExecutes(compileData, "Out of range error");
    }

    [TestMethod, Ignore]
    public void Fail_AppendStringToNumber()
    {
        var code = @"
main
    var a = 3.1 + ""Hello""
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    #endregion
}