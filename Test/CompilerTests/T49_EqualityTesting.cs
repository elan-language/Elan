using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T49_EqualityTesting
{
    #region Passes

    [TestMethod]
    public void Pass_DifferentInstancesWithSameValuesAreEqual()
    {
        var code = @"#
main
    var x = Foo(7, ""Apple"")
    var y = Foo(7, ""Orange"")
    var z = Foo(7, ""Orange"")
    printLine(x is x)
    printLine(x is y)
    printLine(y is z)
end main

class Foo
    constructor(p1 Int, p2 String)
        self.p1 = p1
        self.p2 = p2
    end constructor
    property p1 Int
    property p2 String

    procedure setP1(v Int)
        p1 = v
    end procedure

    function asString() -> String
      return ""{p1} {p2}""
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
    public Foo(int p1, string p2) {
      this.p1 = p1;
      this.p2 = p2;
    }
    public virtual int p1 { get; private set; } = default;
    public virtual string p2 { get; private set; } = """";
    public virtual string asString() {

      return @$""{p1} {p2}"";
    }
    public virtual void setP1(ref int v) {
      p1 = v;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";
      public override void setP1(ref int v) { }
      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo(7, @$""Apple"");
    var y = new Foo(7, @$""Orange"");
    var z = new Foo(7, @$""Orange"");
    printLine(x == x);
    printLine(x == y);
    printLine(y == z);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 7)))) , (expression (value (literal (literalDataStructure ""Apple""))))) )))) (varDef var (assignableValue y) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 7)))) , (expression (value (literal (literalDataStructure ""Orange""))))) )))) (varDef var (assignableValue z) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 7)))) , (expression (value (literal (literalDataStructure ""Orange""))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value x)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value y)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value y)) (binaryOp (conditionalOp is)) (expression (value z)))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( (parameterList (parameter p1 (type Int)) , (parameter p2 (type String))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1))) (assignment (assignableValue (nameQualifier self .) p2) = (expression (value p2)))) end constructor) (property property p1 (type Int)) (property property p2 (type String)) (procedureDef procedure (procedureSignature setP1 ( (parameterList (parameter v (type Int))) )) (statementBlock (assignment (assignableValue p1) = (expression (value v)))) end procedure) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""{p1} {p2}"")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_ActuallyTheSameReference()
    {
        var code = @"#
main
    var x = Foo(7, ""Apple"")
    var y = x
    y.setP1(3)
    var z = Foo(8, ""Orange"")
    printLine(x is x)
    printLine(x is y)
    printLine(x is z)
end main

class Foo
    constructor(p1 Int, p2 String)
        self.p1 = p1
        self.p2 = p2
    end constructor

    property p1 Int
    property p2 String

    procedure setP1(v Int)
        p1 = v
    end procedure

    function asString() -> String
      return ""{p1} {p2}""
    end function
end class
";
        var objectCode = @"";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 7)))) , (expression (value (literal (literalDataStructure ""Apple""))))) )))) (varDef var (assignableValue y) = (expression (value x))) (callStatement (expression (expression (value y)) . (methodCall setP1 ( (argumentList (expression (value (literal (literalValue 3))))) )))) (varDef var (assignableValue z) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 8)))) , (expression (value (literal (literalDataStructure ""Orange""))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value x)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value y)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value z)))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( (parameterList (parameter p1 (type Int)) , (parameter p2 (type String))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1))) (assignment (assignableValue (nameQualifier self .) p2) = (expression (value p2)))) end constructor) (property property p1 (type Int)) (property property p2 (type String)) (procedureDef procedure (procedureSignature setP1 ( (parameterList (parameter v (type Int))) )) (statementBlock (assignment (assignableValue p1) = (expression (value v)))) end procedure) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""{p1} {p2}"")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_StringEquality()
    {
        var code = @"#
main
    var x = ""Apple""
    var y = ""Apple""
    var z = ""apple""
    printLine(x is y)
    printLine(x is z)
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
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_ListEquality()
    {
        var code = @"#
main
    var x = {3,4,5}
    var y = {3,4,5}
    var z = {4,3,5}
    printLine(x is y)
    printLine(x is z)
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
    var x = new StandardLibrary.ElanList<int>(3, 4, 5);
    var y = new StandardLibrary.ElanList<int>(3, 4, 5);
    var z = new StandardLibrary.ElanList<int>(4, 3, 5);
    printLine(x == y);
    printLine(x == z);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 3)) , (literal (literalValue 4)) , (literal (literalValue 5)) })))))) (varDef var (assignableValue y) = (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 3)) , (literal (literalValue 4)) , (literal (literalValue 5)) })))))) (varDef var (assignableValue z) = (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 4)) , (literal (literalValue 3)) , (literal (literalValue 5)) })))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value y)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value z)))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_ArrayEquality()
    {
        var code = @"#
main
    var x = {3,4,5}.asArray()
    var y = {3,4,5}.asArray()
    var z = {4,3,5}.asArray()
    var w = {3,4,5}
    printLine(x is y)
    printLine(x is z)
    printLine(x is w)
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
    var x = asArray(new StandardLibrary.ElanList<int>(3, 4, 5));
    var y = asArray(new StandardLibrary.ElanList<int>(3, 4, 5));
    var z = asArray(new StandardLibrary.ElanList<int>(4, 3, 5));
    var w = new StandardLibrary.ElanList<int>(3, 4, 5);
    printLine(x == y);
    printLine(x == z);
    printLine(x == w);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 3)) , (literal (literalValue 4)) , (literal (literalValue 5)) }))))) . (methodCall asArray ( )))) (varDef var (assignableValue y) = (expression (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 3)) , (literal (literalValue 4)) , (literal (literalValue 5)) }))))) . (methodCall asArray ( )))) (varDef var (assignableValue z) = (expression (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 4)) , (literal (literalValue 3)) , (literal (literalValue 5)) }))))) . (methodCall asArray ( )))) (varDef var (assignableValue w) = (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 3)) , (literal (literalValue 4)) , (literal (literalValue 5)) })))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value y)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value z)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value w)))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_DictionaryEquality()
    {
        var code = @"#
main
    var x = {'a':3,'b':4,'c':5}
    var y = {'a':3,'b':4,'c':5}
    var z = {'b':4,'c':5,'a':3}
    var w = {'a':3,'b':6,'c':5}
    var v = {""b"":4,""c"":5,""a"":3}
    printLine(x is y)
    printLine(x is z)
    printLine(x is w)
    printLine(x is v)
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
    var x = new StandardLibrary.ElanDictionary<char,int>(('a', 3), ('b', 4), ('c', 5));
    var y = new StandardLibrary.ElanDictionary<char,int>(('a', 3), ('b', 4), ('c', 5));
    var z = new StandardLibrary.ElanDictionary<char,int>(('b', 4), ('c', 5), ('a', 3));
    var w = new StandardLibrary.ElanDictionary<char,int>(('a', 3), ('b', 6), ('c', 5));
    var v = new StandardLibrary.ElanDictionary<string,int>((@$""b"", 4), (@$""c"", 5), (@$""a"", 3));
    printLine(x == y);
    printLine(x == z);
    printLine(x == w);
    printLine(x == v);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (value (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 4))) , (literalKvp (literal (literalValue 'c')) : (literal (literalValue 5))) })))))) (varDef var (assignableValue y) = (expression (value (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 4))) , (literalKvp (literal (literalValue 'c')) : (literal (literalValue 5))) })))))) (varDef var (assignableValue z) = (expression (value (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'b')) : (literal (literalValue 4))) , (literalKvp (literal (literalValue 'c')) : (literal (literalValue 5))) , (literalKvp (literal (literalValue 'a')) : (literal (literalValue 3))) })))))) (varDef var (assignableValue w) = (expression (value (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 6))) , (literalKvp (literal (literalValue 'c')) : (literal (literalValue 5))) })))))) (varDef var (assignableValue v) = (expression (value (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalDataStructure ""b"")) : (literal (literalValue 4))) , (literalKvp (literal (literalDataStructure ""c"")) : (literal (literalValue 5))) , (literalKvp (literal (literalDataStructure ""a"")) : (literal (literalValue 3))) })))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value y)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value z)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value w)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) (binaryOp (conditionalOp is)) (expression (value v)))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\nfalse\r\n");
    }

    #endregion

    #region Fails

    #endregion
}