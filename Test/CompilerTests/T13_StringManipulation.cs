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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""Hello"";
    var b = @$""World!"";
    printLine(a + @$"" "" + b);
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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine('_' + @$""Hello"" + '!');
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

    [TestMethod]
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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(@$""Hello"" + 3.1);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""Hello"")))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 3.1)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello3.1\r\n");
    }

    [TestMethod]
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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    printLine(a[2]);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalDataStructure ""abcde""))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (expression (value (literal (literalValue 2)))) ]))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "c\r\n");
    }

    [TestMethod]
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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    printLine(a[(1)..(3)]);
    printLine(a[(2)..]);
    printLine(a[..(2)]);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalDataStructure ""abcde""))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (range (expression (value (literal (literalValue 1)))) .. (expression (value (literal (literalValue 3))))) ]))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (range (expression (value (literal (literalValue 2)))) ..) ]))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (range .. (expression (value (literal (literalValue 2))))) ]))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "bc\r\ncde\r\nab\r\n");
    }

    [TestMethod]
    public void Pass_EqualityTesting()
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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

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

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""abc"")))) (binaryOp (conditionalOp ==)) (expression (value (literal (literalDataStructure ""abc"")))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""abc"")))) (binaryOp (conditionalOp ==)) (expression (value (literal (literalDataStructure ""abcd"")))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""abc"")))) (binaryOp (conditionalOp ==)) (expression (value (literal (literalDataStructure ""Abc"")))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""abc"")))) (binaryOp (conditionalOp is)) (expression (value (literal (literalDataStructure ""abc"")))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""abc"")))) (binaryOp (conditionalOp <>)) (expression (value (literal (literalDataStructure ""abcd"")))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""abc"")))) (binaryOp (conditionalOp is not)) (expression (value (literal (literalDataStructure ""abcd"")))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""abc"")))) (binaryOp (conditionalOp is not)) (expression (value (literal (literalDataStructure ""Abc"")))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\nfalse\r\ntrue\r\ntrue\r\ntrue\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_ComparisonMethods()
    {
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
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(isBefore(@$""abc"", @$""abC""));
    printLine(isAfter(@$""abcd"", @$""abc""));
    printLine(isAfterOrSameAs(@$""abc"", @$""abc""));
    printLine(isBeforeOrSameAs(@$""abc"", @$""abc""));
    printLine(isAfterOrSameAs(@$""abcd"", @$""abc""));
    printLine(isBeforeOrSameAs(@$""abcd"", @$""abc""));
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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""abcde"";
    a = asString((2.1 + 3.4));
    print(a);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalDataStructure ""abcde""))))) (assignment (assignableValue a) = (expression (expression (bracketedExpression ( (expression (expression (value (literal (literalValue 2.1)))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 3.4))))) ))) . (methodCall asString ( )))) (callStatement (expression (methodCall print ( (argumentList (expression (value a))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5.5");
    }

    [TestMethod]
    public void Pass_Interpolation()
    {
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
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    var b = 4;
    var c = @$""{a} x {b} = {a * b}"";
    printLine(c);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalValue 3))))) (varDef var (assignableValue b) = (expression (value (literal (literalValue 4))))) (varDef var (assignableValue c) = (expression (value (literal (literalDataStructure ""{a} x {b} = {a * b}""))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value c))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3 x 4 = 12\r\n");
    }

    [TestMethod]
    public void Pass_UseBracesInString()
    {
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
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    var b = 4;
    var c = @$""{{{a} x {b}}} = {a * b}"";
    printLine(c);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalValue 3))))) (varDef var (assignableValue b) = (expression (value (literal (literalValue 4))))) (varDef var (assignableValue c) = (expression (value (literal (literalDataStructure ""{{{a} x {b}}} = {a * b}""))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value c))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "{3 x 4} = 12\r\n");
    }

    [TestMethod]
    public void Pass_literalNewline()
    {
        var code = @"
main
    var c = ""Hello
 World!""
    printLine(c)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var c = @$""Hello
 World!"";
    printLine(c);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue c) = (expression (value (literal (literalDataStructure ""Hello World!""))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value c))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello\r\n World!\r\n");
    }

    [TestMethod]
    public void Pass_newLineConstant()
    {
        var code = @"
main
    var c = ""Hello ""+ newLine + ""World!""
    printLine(c)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var c = @$""Hello "" + newLine + @$""World!"";
    printLine(c);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue c) = (expression (expression (expression (value (literal (literalDataStructure ""Hello "")))) (binaryOp (arithmeticOp +)) (expression (value newLine))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalDataStructure ""World!"")))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value c))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello \r\nWorld!\r\n");
    }

    [TestMethod]
    public void Pass_AppendStringToNumber()
    {
        var code = @"
main
    var a = 3.1 + ""Hello""
    printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3.1 + @$""Hello"";
    printLine(a);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (expression (value (literal (literalValue 3.1)))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalDataStructure ""Hello"")))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) ))))) end main) <EOF>)";

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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

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
    public void Fail_ComparisonOperators()
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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

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
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

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