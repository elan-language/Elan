﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T55_asString {
    #region Fails

    [TestMethod]
    public void Fail_ClassHasNoAsString() {
        var code = @"#
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String
end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    #endregion

    #region Passes

    [TestMethod]
    public void Pass_AsStringMayBeCalled() {
        var code = @"#
main
    var f = Foo()
    var s = f.asString()
    printLine(s)
end main
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() -> String
         return p2
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
      p2 = @$""Apple"";
    }
    public virtual int p1 { get; set; } = default;
    protected virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return p2;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      protected override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    var s = StandardLibrary.Functions.asString(f);
    printLine(s);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (varDef var (assignableValue s) = (expression (expression (value f)) . (methodCall asString ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value s))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5))))) (assignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple"")))))) end constructor) (property property p1 (type Int)) (property private property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value p2)) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_AsStringCalledWhenObjectPrinted() {
        var code = @"#
main
    var f = Foo()
    printLine(f)
end main
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() -> String
         return p2
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
      p2 = @$""Apple"";
    }
    public virtual int p1 { get; set; } = default;
    protected virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return p2;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      protected override string p2 => """";

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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value f))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5))))) (assignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple"")))))) end constructor) (property property p1 (type Int)) (property private property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value p2)) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_AsStringUsingDefaultHelper() {
        var code = @"#
main
    var f = Foo()
    printLine(f)
end main

class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() -> String
         return self.typeAndProperties()
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
      p2 = @$""Apple"";
    }
    public virtual int p1 { get; set; } = default;
    protected virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return StandardLibrary.Functions.typeAndProperties(this);
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      protected override string p2 => """";

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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value f))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5))))) (assignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple"")))))) end constructor) (property property p1 (type Int)) (property private property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (methodCall (nameQualifier self .) typeAndProperties ( ))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Foo {p1:5, p2:Apple}\r\n");
    }

    [TestMethod]
    public void Pass_AsStringOnVariousDataTypes() {
        var code = @"#
main
    var l = {1,2,3}
    var sl = l.asString()
    printLine(sl)
    var a = {1,2,3}.asArray()
    var sa = a.asString()
    printLine(sa)
    var d = {'a':1, 'b':3, 'z':10}
    var sd = d.asString()
    printLine(sd)
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
    var l = new StandardLibrary.ElanList<int>(1, 2, 3);
    var sl = StandardLibrary.Functions.asString(l);
    printLine(sl);
    var a = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(1, 2, 3));
    var sa = StandardLibrary.Functions.asString(a);
    printLine(sa);
    var d = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
    var sd = StandardLibrary.Functions.asString(d);
    printLine(sd);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue l) = (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 1)) , (literal (literalValue 2)) , (literal (literalValue 3)) })))))) (varDef var (assignableValue sl) = (expression (expression (value l)) . (methodCall asString ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value sl))) )))) (varDef var (assignableValue a) = (expression (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 1)) , (literal (literalValue 2)) , (literal (literalValue 3)) }))))) . (methodCall asArray ( )))) (varDef var (assignableValue sa) = (expression (expression (value a)) . (methodCall asString ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value sa))) )))) (varDef var (assignableValue d) = (expression (value (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 1))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'z')) : (literal (literalValue 10))) })))))) (varDef var (assignableValue sd) = (expression (expression (value d)) . (methodCall asString ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value sd))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {1,2,3}\r\nArray {1,2,3}\r\nDictionary {a:1,b:3,z:10}\r\n");
    }

    #endregion
}