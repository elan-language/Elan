using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_6_ArithmeticExpressions {
    #region Passes

    [TestMethod]
    public void Pass_IntAddition() {
        var code = @"
main
  printLine( 3 + 4)
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
    printLine(3 + 4);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 4)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    [TestMethod]
    public void Pass_IncludeVariable() {
        var code = @"
main
  var a = 3
  printLine( a + 4)
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
    printLine(a + 4);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (literal (literalValue 3))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 4)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_DivideIntegersToFloat() {
        var code = @"
main
  printLine(3/2)
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
        AssertObjectCodeExecutes(compileData, "1.5\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_IntegerDivision() {
        var code = @"
main
  printLine(7 div 2)
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
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_Mod() {
        var code = @"
main
  printLine(11 mod 3)
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
        AssertObjectCodeExecutes(compileData, "2\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_Power() {
        var code = @"
main
  printLine(3 ^ 3)
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
        AssertObjectCodeExecutes(compileData, "81\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_UseVariableBothSides() {
        var code = @"
main
  var a = 3
  a = a + 1
  printLine(a)
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
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_TODO() {
        Assert.Fail("RP to write tests using standard library function calls within expressions, including by dot-syntax");
    }

    #endregion

        #region Fails

        [TestMethod, Ignore]        
        public void Fail_InvalidExpressio() {
            var code = @"
    main
      var a = 3 4
    end main
    ";

            var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
            AssertDoesNotParse(compileData);
        }

        [TestMethod, Ignore]
        public void Fail_PlusEquals() {
            var code = @"
    main
      var a = 3
      a += 1
    end main
    ";

            var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
            AssertDoesNotParse(compileData);
        }

        [TestMethod, Ignore]
        public void Fail_PlusPlus() {
            var code = @"
    main
      var a = 3
      a ++
    end main
    ";

            var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
            AssertDoesNotParse(compileData);
        }

        #endregion
}