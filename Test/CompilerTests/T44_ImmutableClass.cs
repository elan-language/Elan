using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T44_ImmutableClass {
    #region Passes

    [TestMethod]
    public void Pass_BasicImmutableClass() {
        var code = @"#
main
    var f = Foo(3)
    printLine(f.p1)
    printLine(f.square())
end main

immutable class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor
    property p1 Int
    function square() -> Int
        return p1 * p1
    end function
    function asString() -> String
        return """"
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1;
    }
    public virtual int p1 { get; init; } = default;
    public virtual int square() {

      return p1 * p1;
    }
    public virtual string asString() {

      return @$"""";
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
    var f = new Foo(3);
    printLine(f.p1);
    printLine(f.square());
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 3))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . (methodCall square ( )))) ))))) end main) (classDef (immutableClass immutable class Foo (constructor constructor ( (parameterList (parameter p1 (type Int))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature square ( ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp *)) (expression (value p1))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n9\r\n");
    }

    [TestMethod]
    public void Pass_AbstractImmutableClass() {
        var code = @"#
main
    var f = Foo(3)
    printLine(f.p1)
    printLine(f.square())
end main

abstract immutable class Bar
    property p1 Int
    function square() -> Int
end class

immutable class Foo inherits Bar
    constructor(p1 Int)
        self.p1 = p1
    end constructor
    property p1 Int
    function square() -> Int
        return p1 * p1
    end function 
    function asString() -> String
        return """"
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public interface Bar {
    public int p1 { get; }
    public int square();
  }
  public record class Foo : Bar {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1;
    }
    public virtual int p1 { get; init; } = default;
    public virtual int square() {

      return p1 * p1;
    }
    public virtual string asString() {

      return @$"""";
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
    var f = new Foo(3);
    printLine(f.p1);
    printLine(f.square());
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 3))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . (methodCall square ( )))) ))))) end main) (classDef (abstractImmutableClass abstract immutable class Bar (property property p1 (type Int)) function (functionSignature square ( ) -> (type Int)) end class)) (classDef (immutableClass immutable class Foo (inherits inherits (type Bar)) (constructor constructor ( (parameterList (parameter p1 (type Int))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature square ( ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp *)) (expression (value p1))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n9\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_ProcedureMethod() {
        var code = @"#
immutable class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure
    
    function asString() -> String
        return """"
    end function
end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_ProcedureMethodOnAbstractImmutableClass() {
        var code = @"#
abstract immutable class Bar
    property p1 Int

    procedure setP1(v Int)
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_AbstractAndImmutableReversed() {
        var code = @"#
immutable abstract class Bar
    property p1 Int

    procedure setP1(v Int)
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}