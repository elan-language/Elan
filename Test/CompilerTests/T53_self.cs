using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T53_self {
    #region Passes

    [TestMethod]
    public void Pass_DisambiguateParamAndProperty() {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 Int

    function asString() -> String
        return """"
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
      this.p1 = p1;
    }
    public virtual int p1 { get; set; } = default;
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
    var x = new Foo(7);
    printLine(x.p1);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 7))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( (parameterList (parameter p1 (type Int))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    [TestMethod]
    public void Pass_UsingSelfAsAnInstance() {
        var code = @"#
main
    var f = Foo()
    printLine(f.bar())
end main

function doubled(f Foo) -> Int
    return 2 * f.p1
end function

class Foo
    constructor()
        p1 = 3
    end constructor

    property p1 Int

    function bar() -> Int
        return doubled(self)
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
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int doubled(Foo f) {

    return 2 * f.p1;
  }
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 3;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int bar() {

      return Globals.doubled(this);
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
    var f = new Foo();
    printLine(f.bar());
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . (methodCall bar ( )))) ))))) end main) (functionDef (functionWithBody function (functionSignature doubled ( (parameterList (parameter f (type Foo))) ) -> (type Int)) statementBlock return (expression (expression (value (literal (literalValue 2)))) (binaryOp (arithmeticOp *)) (expression (expression (value f)) . p1)) end function)) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 3)))))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature bar ( ) -> (type Int)) statementBlock return (expression (methodCall doubled ( (argumentList (expression (value self))) ))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "6\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoSuchProperty() {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        self.p = p1
    end constructor

    property p1 Int

    function asString() -> String
        return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_MissingSelfCausesCompileErrorDueToAssigningToParam() {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        p1 = p1
    end constructor

    property p1 Int

    function asString() -> String
        return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    #endregion
}