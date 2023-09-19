using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_5_Variables {
    #region Passes

    [TestMethod]
    public void Pass_Int() {
        var code = @"
main
  var a = 3
  printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    printLine(a);
  }
}";

        var parseTree = @"(file (main  main (statementBlock (varDef  var (assignableValue a) = (expression (value (literal (literalValue 3))))) (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_Reassign() {
        var code = @"
main
  var a = 3
  a = 4
  printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    a = 4;
    printLine(a);
  }
}";

        var parseTree = @"(file (main  main (statementBlock (varDef  var (assignableValue a) = (expression (value (literal (literalValue 3))))) (assignment  (assignableValue a) = (expression (value (literal (literalValue 4))))) (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    [TestMethod]
    public void Pass_CoerceFloatToIntVar() {
        var code = @"
main
  var a = 3.1
  a = 4
  printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 3.1;
    a = 4;
    printLine(a);
  }
}";

        var parseTree = @"(file (main  main (statementBlock (varDef  var (assignableValue a) = (expression (value (literal (literalValue 3.1))))) (assignment  (assignableValue a) = (expression (value (literal (literalValue 4))))) (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void fail_wrongKeyword() {
        var code = @"
main
  variable a = 3
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_duplicateVar() {
        var code = @"
main
  var a = 3
  var a = 4
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void fail_duplicateVarConst() {
        var code = @"
main
  constant a = 3
  var a = 4
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void fail_GlobalVariable() {
        var code = @"
var a = 4
main
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_assignIncompatibleType() {
        var code = @"
main
  var a = 4
  a = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void fail_notInitialised() {
        var code = @"
main
  var a
  a = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_invalidVariableName1() {
        var code = @"
main
  var A = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_invalidVariableName2() {
        var code = @"
main
  var a@b = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_useOfKeywordAsName() {
        var code = @"
main
  var new = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}