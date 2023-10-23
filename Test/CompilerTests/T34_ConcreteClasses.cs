using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T34_ConcreteClasses
{
    #region Passes

    [TestMethod]
    public void Pass_Class_SimpleInstantiation_PropertyAccess_Methods()
    {
        var code = @"#
main
    var x = Foo()
    printLine(x.p1)
    printLine(x.p2)
    printLine(x.asString())
end main

class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 as Int

    property p2 as String

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
    public Foo() {
      p1 = 5;
    }
    public int p1 { get; set; }
    public string p2 { get; set; } = """";
    public string asString() {

      return @$"""";
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo();
    printLine(x.p1);
    printLine(x.p2);
    printLine(asString(x));
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p2)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . (methodCall asString ( )))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5)))))) end constructor) (property property p1 as (type Int)) (property property p2 as (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) as (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n\r\n\r\n"); //N.B. Important that String prop should be auto-initialised to "" not null
    }

    [TestMethod]
    public void Pass_ConstructorWithParm()
    {
        var code = @"#
main
    var x = Foo(7, ""Apple"")
    printLine(x.p1)
    printLine(x.p2)
end main

class Foo
    constructor(p_1 Int, p_2 String)
        p1 = p_1
        p2 = p_2
    end constructor

    property p1 as Int
    property p2 as String
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
    public Foo(int p_1, string p_2) {
      p1 = p_1;
      p2 = p_2;
    }
    public int p1 { get; set; }
    public string p2 { get; set; } = """";
    public string asString() {

      return @$"""";
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo(7, @$""Apple"");
    printLine(x.p1);
    printLine(x.p2);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 7)))) , (expression (value (literal (literalDataStructure ""Apple""))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p2)) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( (parameterList (parameter p_1 (type Int)) , (parameter p_2 (type String))) ) (statementBlock (assignment (assignableValue p1) = (expression (value p_1))) (assignment (assignableValue p2) = (expression (value p_2)))) end constructor) (property property p1 as (type Int)) (property property p2 as (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) as (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\nApple\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    #endregion

    #region Fails
    public void Fail_NoConstructor()
    {
        var code = @"#
class Foo

    property p1 as Int
    property p2 as String
   
    function asString() as String
        return """"
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    public void Fail_InitialisePropertyInLine()
    {
        var code = @"#
class Foo

    property p1 as Int = 3
    property p2 as String
   
    function asString() as String
        return """"
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_AttemptToModifyAPropertyDirectly()
    {
        var code = @"#
main
    var x = Foo()
    x.p1 = 3
end main

class Foo
    constructor()
    end constructor

    property p1 as Int

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

    [TestMethod, Ignore]
    public void Fail_OverloadedConstructor()
    {
        var code = @"#

class Foo
    constructor()
    end constructor

    constructor(val Int)
        p1 = val
    end constructor

    property p1 as Int

    function asString() as String
        return """"
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_InstantiateWithoutRequiredArgs()
    {
        var code = @"#
main
    var x = Foo()
end main

class Foo
    constructor(val Int)
        p1 = val
    end constructor

    property p1 as Int

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

    [TestMethod, Ignore]
    public void Fail_InstantiateWithWrongArgType()
    {
        var code = @"#
main
    var x = Foo(7.1)
end main

class Foo
    constructor(val Int)
        val = p1
    end constructor

    property p1 as Int

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

    [TestMethod, Ignore]
    public void Fail_SupplyingArgumentNotSpecified()
    {
        var code = @"#
main
    var x = Foo(7)
end main

class Foo
    constructor()
    end constructor

    property p1 as Int

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