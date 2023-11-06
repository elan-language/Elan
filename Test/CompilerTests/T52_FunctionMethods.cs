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

    [TestMethod]
    public void Pass_FunctionMethodMayCallOtherClassFunctionMethod() {
        var code = @"#
main
    var f = Foo()
    var b = Bar()
    printLine(f.times(b))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 Int

    function times(b Bar) -> Int
        return p1PlusOne() * b.p1PlusOne()
    end function

    function p1PlusOne() -> Int
        return p1 +1 
    end function

    function asString() -> String
         return """"
    end function

end class

class Bar
    constructor()
        p1 = 1
    end constructor

    property p1 Int

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
    public virtual int times(Bar b) {

      return p1PlusOne() * b.p1PlusOne();
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
  public record class Bar {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 1;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int p1PlusOne() {

      return p1 + 1;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override int p1 => default;

      public override string asString() { return ""default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    var b = new Bar();
    printLine(f.times(b));
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (varDef var (assignableValue b) = (expression (newInstance (type Bar) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . (methodCall times ( (argumentList (expression (value b))) )))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature times ( (parameterList (parameter b (type Bar))) ) -> (type Int)) statementBlock return (expression (expression (methodCall p1PlusOne ( ))) (binaryOp (arithmeticOp *)) (expression (expression (value b)) . (methodCall p1PlusOne ( )))) end function)) (functionDef (functionWithBody function (functionSignature p1PlusOne ( ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) (classDef (mutableClass class Bar (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 1)))))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature p1PlusOne ( ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp +)) (expression (value (literal (literalValue 1))))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_FunctionMethodNameHidesGlobalFunction() {
        var code = @"#
main
    var f = Foo()
    printLine(f)
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 Int

    function asString() -> String
         return p1.asString()
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
    public virtual string asString() {

      return StandardLibrary.Functions.asString(p1);
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
    printLine(f);
  }
}";

        var parseTree = @"*";
        
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