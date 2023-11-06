using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T54_With {
    #region Fails

    [TestMethod]
    public void Fail_NonMatchingProperty() {
        var code = @"#
main
    var x = Foo() with {p1 = 3, p3 = ""Apple"" }
    printLine(x.p1)
    printLine(x.p2)
end main

class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 Int

    property p2 String

    function asString() -> String
         return """"
    end function

end class
";
        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( ) (withClause with { (inlineAsignment (assignableValue p1) = (expression (value (literal (literalValue 3))))) , (inlineAsignment (assignableValue p3) = (expression (value (literal (literalDataStructure ""Apple""))))) })))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p2)) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 (type Int)) (property property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion

    #region Passes

    [TestMethod]
    public void Pass_InstantiatingClassWithZeroParamConstructor() {
        var code = @"#
main
    var x = Foo() with {p1 = 3, p2 = ""Apple"" }
    printLine(x.p1)
    printLine(x.p2)
end main

class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 Int

    property p2 String

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

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo() with {p1 = 3, p2 = @$""Apple""};
    printLine(x.p1);
    printLine(x.p2);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( ) (withClause with { (inlineAsignment (assignableValue p1) = (expression (value (literal (literalValue 3))))) , (inlineAsignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple""))))) })))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p2)) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 (type Int)) (property property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_ConstructorWithParm() {
        var code = @"#
main
    var x = Foo(7) with {p1 = 3, p2 = ""Apple"" }
    printLine(x.p1)
    printLine(x.p2)
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor
    property p1 Int

    property p2 String

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
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo(7) with {p1 = 3, p2 = @$""Apple""};
    printLine(x.p1);
    printLine(x.p2);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 7))))) ) (withClause with { (inlineAsignment (assignableValue p1) = (expression (value (literal (literalValue 3))))) , (inlineAsignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple""))))) })))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p2)) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( (parameterList (parameter p1 (type Int))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end constructor) (property property p1 (type Int)) (property property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_AppliedToInstanceButReturnedOneIsNewInstance() {
        var code = @"#
main
    var x = Foo()
    var y = x with {p1 = 3, p2 = ""Apple"" }
    printLine(y.p1)
    printLine(y.p2)
    printLine(x.p1)
end main

class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 Int

    property p2 String

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

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo();
    var y = x with {p1 = 3, p2 = @$""Apple""};
    printLine(y.p1);
    printLine(y.p2);
    printLine(x.p1);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( )))) (varDef var (assignableValue y) = (expression (expression (value x)) (withClause with { (inlineAsignment (assignableValue p1) = (expression (value (literal (literalValue 3))))) , (inlineAsignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple""))))) }))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value y)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value y)) . p2)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 (type Int)) (property property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n5\r\n");
    }

    [TestMethod]
    public void Pass_WorksWithImmutableClass() {
        var code = @"#
main
    var x = Foo()
    var y = x with {p1 = 3, p2 = ""Apple"" }
    printLine(y.p1)
    printLine(y.p2)
    printLine(x.p1)
end main

immutable class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 Int

    property p2 String

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

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; init; } = default;
    public virtual string p2 { get; init; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo();
    var y = x with {p1 = 3, p2 = @$""Apple""};
    printLine(y.p1);
    printLine(y.p2);
    printLine(x.p1);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( )))) (varDef var (assignableValue y) = (expression (expression (value x)) (withClause with { (inlineAsignment (assignableValue p1) = (expression (value (literal (literalValue 3))))) , (inlineAsignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple""))))) }))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value y)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value y)) . p2)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) ))))) end main) (classDef (immutableClass immutable class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 (type Int)) (property property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n5\r\n");
    }

    #endregion
}