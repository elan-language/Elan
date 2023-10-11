using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T15_ForInLoop {
    #region Passes

    [TestMethod]
    public void Pass_List() {
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
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.List<int>(7, 8, 9);
    var n = 0;
    foreach (var x in a) {
      n = n + x;
    }
    printLine(n);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 7)) , (literal (literalValue 8)) , (literal (literalValue 9)) })))))) (varDef var (assignableValue n) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (forIn for x in (expression (value a)) (statementBlock (assignment (assignableValue n) = (expression (expression (value n)) (binaryOp (arithmeticOp +)) (expression (value x))))) end for)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value n))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "24\r\n");
    }

    [TestMethod]
    public void Pass_Array() {
        var code = @"
main
    var a = {7,8,9}.asArray()
    var n = 0
    for x in a
        n = n + x
    end for
    printLine(n)
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
    var a = asArray(new StandardLibrary.List<int>(7, 8, 9));
    var n = 0;
    foreach (var x in a) {
      n = n + x;
    }
    printLine(n);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 7)) , (literal (literalValue 8)) , (literal (literalValue 9)) }))))) . (methodCall asArray ( )))) (varDef var (assignableValue n) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (forIn for x in (expression (value a)) (statementBlock (assignment (assignableValue n) = (expression (expression (value n)) (binaryOp (arithmeticOp +)) (expression (value x))))) end for)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value n))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "24\r\n");
    }

    [TestMethod]
    public void Pass_string() {
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
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""hello"";
    foreach (var x in a) {
      printLine(x);
    }
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalDataStructure ""hello""))))) (proceduralControlFlow (forIn for x in (expression (value a)) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value x))) ))))) end for))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "h\r\ne\r\nl\r\nl\r\no\r\n");
    }

    [TestMethod]
    public void Pass_doubleLoop() {
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
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    foreach (var x in @$""12"") {
      foreach (var y in @$""34"") {
        printLine(@$""{x}{y }"");
      }
    }
  }
}";

        var parseTree = @"(file (main main (statementBlock (proceduralControlFlow (forIn for x in (expression (value (literal (literalDataStructure ""12"")))) (statementBlock (proceduralControlFlow (forIn for y in (expression (value (literal (literalDataStructure ""34"")))) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""{x}{y }""))))) ))))) end for))) end for))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "13\r\n14\r\n23\r\n24\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_variableIsScoped() {
        var code = @"
main
    var a = {7,8,9}
    var x = ""hello"";
    for x in a
       printLine(x)
    end for
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
    var a = new StandardLibrary.List<int>(7, 8, 9);
    var x = @$""hello"";
    foreach (var x in a) {
      printLine(x);
    }
    printLine(x);
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
    public void Fail_NoEndFor() {
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
    public void Fail_applyToANonIterable() {
        var code = @"
main
    var y = 10
    for x in y
       printLine(x)
    end for
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
    public void Fail_CannotAlterTheIterableWithinLoop() {
        var code = @"
main
  var a ={1,2,3,4,5}
  for x in a
    a = a + x
  end for
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