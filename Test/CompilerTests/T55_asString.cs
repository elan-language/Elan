using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T55_asString
{
    #region Passes

    [TestMethod]
    public void Pass_AsStringMayBeCalled()
    {
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

    property p1 as Int

    private property p2 as String

    function asString() as String
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
  public class Foo {
    public Foo() {
      p1 = 5;
      p2 = @$""Apple"";
    }
    public int p1 { get; set; }
    private string p2 { get; set; } = """";
    public string asString() {

      return p2;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    var s = asString(f);
    printLine(s);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (varDef var (assignableValue s) = (expression (expression (value f)) . (methodCall asString ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value s))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5))))) (assignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple"")))))) end constructor) (property property p1 as (type Int)) (property private property p2 as (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) as (type String)) statementBlock return (expression (value p2)) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_AsStringCalledWhenObjectPrinted()
    {
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

    property p1 as Int

    private property p2 as String

    function asString() as String
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
  public class Foo {
    public Foo() {
      p1 = 5;
      p2 = @$""Apple"";
    }
    public int p1 { get; set; }
    private string p2 { get; set; } = """";
    public string asString() {

      return p2;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value f))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5))))) (assignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple"")))))) end constructor) (property property p1 as (type Int)) (property private property p2 as (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) as (type String)) statementBlock return (expression (value p2)) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_AsStringUsingDefaultHelper()
    {
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

    property p1 as Int

    private property p2 as String

    function asString() as String
         var a = self.typeAndProperties()
         return a
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
    public Foo() {
      p1 = 5;
      p2 = @$""Apple"";
    }
    public int p1 { get; set; }
    private string p2 { get; set; } = """";
    public string asString() {
      var a = typeAndProperties(this);
      return a;
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
        AssertObjectCodeExecutes(compileData, "Foo {p1:5, p2:Apple}\r\n");
    }


    [TestMethod]
    public void Pass_AsStringOnVariousDataTypes()
    {
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
    var sl = asString(l);
    printLine(sl);
    var a = asArray(new StandardLibrary.ElanList<int>(1, 2, 3));
    var sa = asString(a);
    printLine(sa);
    var d = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
    var sd = asString(d);
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

    #region Fails

    [TestMethod]
    public void Fail_ClassHasNoAsString()
    {
        var code = @"#
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String
end class
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    #endregion
}