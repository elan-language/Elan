using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_8_ForLoop {
    [TestMethod]
    public void Pass_minimal() {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
  end for
  printLine(tot)
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
    var tot = 0;
    for (var i = 1; i <= 10; i = i + 1) {
      tot = tot + i;
    }
    printLine(tot);
  }
}"; // could be n++ when there is no step specified, whichever is easier

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue tot) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (for for i = (expression (value (literal (literalValue 1)))) to (expression (value (literal (literalValue 10)))) (statementBlock (assignment (assignableValue tot) = (expression (expression (value tot)) (binaryOp (arithmeticOp +)) (expression (value i))))) end for)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value tot))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "55\r\n");
    }

    [TestMethod]
    public void Pass_withStep() {
        var code = @"
main
  var tot = 0
  for i = 1 to 10 step 2
    tot = tot + i
  end for
  printLine(tot)
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
    var tot = 0;
    for (var i = 1; i <= 10; i = i + 2) {
      tot = tot + i;
    }
    printLine(tot);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue tot) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (for for i = (expression (value (literal (literalValue 1)))) to (expression (value (literal (literalValue 10)))) step 2 (statementBlock (assignment (assignableValue tot) = (expression (expression (value tot)) (binaryOp (arithmeticOp +)) (expression (value i))))) end for)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value tot))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "25\r\n");
    }

    [TestMethod]
    public void Pass_negativeStep() {
        var code = @"
main
  var tot = 0
  for i = 10 to 3 step -1
    tot = tot + i
  end for
  printLine(tot)
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
    var tot = 0;
    for (var i = 10; i >= 3; i = i - 1) {
      tot = tot + i;
    }
    printLine(tot);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue tot) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (for for i = (expression (value (literal (literalValue 10)))) to (expression (value (literal (literalValue 3)))) step - 1 (statementBlock (assignment (assignableValue tot) = (expression (expression (value tot)) (binaryOp (arithmeticOp +)) (expression (value i))))) end for)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value tot))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "52\r\n");
    }

    [TestMethod]
    public void Pass_innerLoop() {
        var code = @"
main
  var tot = 0
  for i = 1 to 3
    for j = 1 to 4
      tot = tot + 1
    end for
  end for
  printLine(tot)
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
    var tot = 0;
    for (var i = 1; i <= 3; i = i + 1) {
      for (var j = 1; j <= 4; j = j + 1) {
        tot = tot + 1;
      }
    }
    printLine(tot);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue tot) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (for for i = (expression (value (literal (literalValue 1)))) to (expression (value (literal (literalValue 3)))) (statementBlock (proceduralControlFlow (for for j = (expression (value (literal (literalValue 1)))) to (expression (value (literal (literalValue 4)))) (statementBlock (assignment (assignableValue tot) = (expression (expression (value tot)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))))) end for))) end for)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value tot))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_canUseExistingVariablesOfRightType() {
        var code = @"
main
  var lower = 1
  var upper = 10
  var tot = 0
  for i = lower to upper step 2 
    tot = tot + i
  end for
  printLine(tot)
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
    var lower = 1;
    var upper = 10;
    var tot = 0;
    for (var i = lower; i <= upper; i = i + 2) {
      tot = tot + i;
    }
    printLine(tot);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue lower) = (expression (value (literal (literalValue 1))))) (varDef var (assignableValue upper) = (expression (value (literal (literalValue 10))))) (varDef var (assignableValue tot) = (expression (value (literal (literalValue 0))))) (proceduralControlFlow (for for i = (expression (value lower)) to (expression (value upper)) step 2 (statementBlock (assignment (assignableValue tot) = (expression (expression (value tot)) (binaryOp (arithmeticOp +)) (expression (value i))))) end for)) (callStatement (expression (methodCall printLine ( (argumentList (expression (value tot))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "25\r\n");
    }

    [TestMethod]
    public void Fail_useOfFloat() {
        var code = @"
main
  var tot = 0
  for i = 1.5 to 10
    tot = tot + i
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
    public void Fail_modifyingCounter() {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    i = 10
  end for
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    public void Fail_missingEnd() {
        var code = @"
main
  var tot = 0
  for i = 1 to 3
    for j = 1 to 4
      tot = tot + 1
    end for
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_nextVariable() {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_break() {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
    break
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_continue() {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
    continue
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}