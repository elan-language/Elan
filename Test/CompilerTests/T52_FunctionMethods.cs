using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T52_FunctionMethods {
    #region Passes

    [TestMethod]
    public void Pass_HappyCase() {
        var code = @"#
main
    var f = Foo()
    printLine(f.times(2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        return p1 * value
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
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int times(int value) {

      return p1 * value;
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
    printLine(f.times(2));
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . (methodCall times ( (argumentList (expression (value (literal (literalValue 2))))) )))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature times ( (parameterList (parameter value (type Int))) ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp *)) (expression (value value))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_FunctionMethodMayCallOtherFunctionMethod() {
        var code = @"#
main
    var f = Foo()
    printLine(f.times(2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        return p1PlusOne() * value
    end function

    function p1PlusOne() -> Int
        return p1 +1 
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
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int times(int value) {

      return p1PlusOne() * value;
    }
    public virtual int p1PlusOne() {

      return p1 + 1;
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
    printLine(f.times(2));
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . (methodCall times ( (argumentList (expression (value (literal (literalValue 2))))) )))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature times ( (parameterList (parameter value (type Int))) ) -> (type Int)) statementBlock return (expression (expression (methodCall p1PlusOne ( ))) (binaryOp (arithmeticOp *)) (expression (value value))) end function)) (functionDef (functionWithBody function (functionSignature p1PlusOne ( ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_FunctionCannotBeCalledDirectly() {
        var code = @"#
main
    var f = Foo()
    printLine(times(f,2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        return p1 * value
    end function

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

    [TestMethod]
    public void Fail_FunctionMethodCannotMutateProperty() {
        var code = @"#
class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        p1 = p1 * value
        return p1
    end function

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

    [TestMethod]
    public void Fail_FunctionMethodCannotCallProcedureMethod() {
        var code = @"#
class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        setP1(p1 * value)
        return p1
    end function

    procedure setP1(value Int) 
        p1 = value
    end procedure

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