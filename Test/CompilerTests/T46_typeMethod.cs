using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T46_typeMethod {
    #region Passes

    [TestMethod]
    public void Pass_Template() {
        var code = @"
main
    printLine(1.type())
    printLine(1.1.type())
    printLine('a'.type())
    printLine(false.type())
    printLine(""a"".type())
    printLine({1,2,3}.type())
    printLine({'a': 3}.type())
    printLine({1,2,3}.asArray().type())
    printLine(Foo(3).type())
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1 * 2
    end constructor

    property p1 Int

    function asString() -> String
        return ""{p1}""
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1 * 2;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string asString() {

      return @$""{p1}"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    printLine(type(1));
    printLine(type(1.1));
    printLine(type('a'));
    printLine(type(false));
    printLine(type(@$""a""));
    printLine(type(new StandardLibrary.ElanList<int>(1, 2, 3)));
    printLine(type(new StandardLibrary.ElanDictionary<char,int>(('a', 3))));
    printLine(type(asArray(new StandardLibrary.ElanList<int>(1, 2, 3))));
    printLine(type(new Foo(3)));
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 1)))) . (methodCall type ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 1.1)))) . (methodCall type ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 'a')))) . (methodCall type ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue false)))) . (methodCall type ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure ""a"")))) . (methodCall type ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 1)) , (literal (literalValue 2)) , (literal (literalValue 3)) }))))) . (methodCall type ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 3))) }))))) . (methodCall type ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 1)) , (literal (literalValue 2)) , (literal (literalValue 3)) }))))) . (methodCall asArray ( ))) . (methodCall type ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 3))))) ))) . (methodCall type ( )))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( (parameterList (parameter p1 (type Int))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (expression (value p1)) (binaryOp (arithmeticOp *)) (expression (value (literal (literalValue 2))))))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""{p1}"")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Int\r\nFloat\r\nChar\r\nBool\r\nString\r\nList<Int>\r\nDictionary<Char,Int>\r\nArray<Int>\r\nFoo\r\n");
    }

    [TestMethod]
    public void Pass_TypeTesting() {
        var code = @"
main
    printLine(1.type() is 2.type());
    printLine(1.0.type() is 2.type());
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1 * 2
    end constructor

    property p1 Int

    function asString() -> String
        return ""{p1}""
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1 * 2;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string asString() {

      return @$""{p1}"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    printLine(type(1) == type(2));
    printLine(type(1.0) == type(2));
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value (literal (literalValue 1)))) . (methodCall type ( ))) (binaryOp (conditionalOp is)) (expression (expression (value (literal (literalValue 2)))) . (methodCall type ( ))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value (literal (literalValue 1.0)))) . (methodCall type ( ))) (binaryOp (conditionalOp is)) (expression (expression (value (literal (literalValue 2)))) . (methodCall type ( ))))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( (parameterList (parameter p1 (type Int))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (expression (value p1)) (binaryOp (arithmeticOp *)) (expression (value (literal (literalValue 2))))))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""{p1}"")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    #endregion

    #region Fails

    #endregion
}