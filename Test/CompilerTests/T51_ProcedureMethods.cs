using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T51_ProcedureMethods
{
    #region Passes

    [TestMethod]
    public void Pass_HappyCase()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.p1)
    f.setP1(7)
    printLine(f.p1)
end main

class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 as Int
    procedure setP1(value Int)
        p1 = value
    end procedure
    function asString() as String
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
  public class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public int p1 { get; private set; } = default;
    public virtual string asString() {

      return @$"""";
    }
    public virtual void setP1(ref int value) {
      p1 = value;
    }
    private class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override void setP1(ref int value) { }
      public override string asString() { return ""Default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f.p1);
    var _setP1_0 = 7;
    f.setP1(ref _setP1_0);
    printLine(f.p1);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . p1)) )))) (callStatement (expression (expression (value f)) . (methodCall setP1 ( (argumentList (expression (value (literal (literalValue 7))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . p1)) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 as (type Int)) (procedureDef procedure (procedureSignature setP1 ( (parameterList (parameter value (type Int))) )) (statementBlock (assignment (assignableValue p1) = (expression (value value)))) end procedure) (functionDef (functionWithBody function (functionSignature asString ( ) as (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n7\r\n");
    }

    [TestMethod]
    public void Pass_ProcedureCanContainSystemCall()
    {
        var code = @"#
main
    var f = Foo()
    f.display()
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    procedure display()
        printLine(p1)
    end procedure

    function asString() as String
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
  public class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public int p1 { get; private set; } = default;
    public virtual string asString() {

      return @$"""";
    }
    public virtual void display() {
      printLine(p1);
    }
    private class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override void display() { }
      public override string asString() { return ""Default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    f.display();
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (expression (value f)) . (methodCall display ( ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 as (type Int)) (procedureDef procedure (procedureSignature display ( )) (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value p1))) ))))) end procedure) (functionDef (functionWithBody function (functionSignature asString ( ) as (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n");
    }
    #endregion

    #region Fails

    [TestMethod]
    public void Fail_ProcedureCannotBeCalledDirectly()
    {
        var code = @"#
main
    var f = Foo()
    display(f)
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    procedure display()
        printLine(p1)
    end procedure

    function asString() as String
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