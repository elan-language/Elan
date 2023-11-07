using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T15_ForEachLoop {
    #region Passes

    [TestMethod]
    public void Pass_List() {
        var code = @"
main
    var a = {7,8,9}
    var n = 0
    foreach x in a
        n = n + x
    end foreach
    printLine(n)
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
    var a = new StandardLibrary.ElanList<int>(7, 8, 9);
    var n = 0;
    foreach (var x in a) {
      n = n + x;
    }
    printLine(n);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 7)) , (literal (literalValue 8)) , (literal (literalValue 9)) })))))) (varDef var (assignableValue n) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (foreach foreach x in (expression (value a)) (statementBlock (assignment (assignableValue n) = (expression (expression (value n)) (binaryOp (arithmeticOp +)) (expression (value x))))) end foreach)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value n))) ))))) end main) <EOF>)";

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
    foreach x in a
        n = n + x
    end foreach
    printLine(n)
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
    var a = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(7, 8, 9));
    var n = 0;
    foreach (var x in a) {
      n = n + x;
    }
    printLine(n);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 7)) , (literal (literalValue 8)) , (literal (literalValue 9)) }))))) . (methodCall asArray ( )))) (varDef var (assignableValue n) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (foreach foreach x in (expression (value a)) (statementBlock (assignment (assignableValue n) = (expression (expression (value n)) (binaryOp (arithmeticOp +)) (expression (value x))))) end foreach)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value n))) ))))) end main) <EOF>)";

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
    foreach x in a
        printLine(x)
    end foreach
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
    var a = @$""hello"";
    foreach (var x in a) {
      printLine(x);
    }
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalDataStructure ""hello""))))) (proceduralControlFlow (foreach foreach x in (expression (value a)) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value x))) ))))) end foreach))) end main) <EOF>)";

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
    foreach x in ""12""
        foreach y in ""34""
            printLine(""{x}{y }"")
        end foreach
    end foreach
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
    foreach (var x in @$""12"") {
      foreach (var y in @$""34"") {
        printLine(@$""{x}{y }"");
      }
    }
  }
}";

        var parseTree = @"(file (main main (statementBlock (proceduralControlFlow (foreach foreach x in (expression (value (literal (literalDataStructure ""12"")))) (statementBlock (proceduralControlFlow (foreach foreach y in (expression (value (literal (literalDataStructure ""34"")))) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""{x}{y }""))))) ))))) end foreach))) end foreach))) end main) <EOF>)";

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
    foreach x in a
       printLine(x)
    end foreach
    printLine(x)
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
    var a = new StandardLibrary.ElanList<int>(7, 8, 9);
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
    public void Fail_NoEndForeach() {
        var code = @"
main
  var a = ""hello""
  foreach x in a
   printLine(x)
  end for
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
    foreach x in y
       printLine(x)
    end foreach
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
  foreach x in a
    a = a + x
  end foreach
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify control variable");
    }

    #endregion
}