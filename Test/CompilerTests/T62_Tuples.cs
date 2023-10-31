using Compiler;
using StandardLibrary;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T62_Tuples
{
    #region Passes

    [TestMethod, Ignore]
    public void Pass_CreatingTuplesAndReadingContents()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    printLine(x)
    printLine(x[0])
    printLine(x[1])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = (3, @$""Apple"");
    printLine(x);
    printLine(x[0]);
    printLine(x[1]);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalDataStructure (literalTuple ( (literal (literalValue 3)) , (literal (literalDataStructure ""Apple"")) ))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value x))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (index [ (expression (value (literal (literalValue 0)))) ]))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (index [ (expression (value (literal (literalValue 1)))) ]))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(3,Apple)\r\n3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_DeconstructIntoExistingVariables()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 0
    var z = """"
    (y, z) = x
    printLine(y)
    printLine(z)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalDataStructure (literalTuple ( (literal (literalValue 3)) , (literal (literalDataStructure ""Apple"")) ))))))) (varDef var (assignableValue y) = (expression (value (literal (literalValue 0))))) (varDef var (assignableValue z) = (expression (value (literal (literalDataStructure """"))))) (assignment (assignableValue (deconstructedTuple ( y , z ))) = (expression (value x))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value z))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_DeconstructIntoNewVariables()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    (var y, var z) = x
    printLine(y)
    printLine(z)
end main
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalDataStructure (literalTuple ( (literal (literalValue 3)) , (literal (literalDataStructure ""Apple"")) ))))))) (assignment (assignableValue (deconstructedTuple ( var y , var z ))) = (expression (value x))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value z))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfSameType()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    x = (4,""Pear"")
    printLine(x)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalDataStructure (literalTuple ( (literal (literalValue 3)) , (literal (literalDataStructure ""Apple"")) ))))))) (assignment (assignableValue x) = (expression (value (literal (literalDataStructure (literalTuple ( (literal (literalValue 4)) , (literal (literalDataStructure ""Pear"")) ))))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value x))) ))))) end main) <EOF>)";

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
    [TestMethod, Ignore]
    public void Fail_OutOfRangeError()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    printLine(x[2])
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Some error");
    }

    [TestMethod, Ignore]
    public void Fail_AssignItemToWrongType()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 4
    y = x[1]
end main
";
        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_ImmutableSoCannotAssignAnItem()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    x[0] = 4
end main
";
        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }


    [TestMethod]
    public void Fail_DeconstructIntoWrongType()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 0
    var z = """"
    (z, y) = x
    printLine(y)
    printLine(z)
end main
";
        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalDataStructure (literalTuple ( (literal (literalValue 3)) , (literal (literalDataStructure ""Apple"")) ))))))) (varDef var (assignableValue y) = (expression (value (literal (literalValue 0))))) (varDef var (assignableValue z) = (expression (value (literal (literalDataStructure """"))))) (assignment (assignableValue (deconstructedTuple ( z , y ))) = (expression (value x))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value y))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value z))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfWrongType()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    x = (""4"",""Pear"")
    printLine(x)
end main
";
       
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }
    #endregion
}