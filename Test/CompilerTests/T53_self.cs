using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T53_self
{
    #region Passes

    [TestMethod]
    public void Pass_DisambiguateParamAndProperty()
    {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 as Int

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
    public Foo(int p1) {
      this.p1 = p1;
    }
    public int p1 { get; set; }
    public string asString() {

      return @$"""";
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo(7);
    printLine(x.p1);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( (argumentList (expression (value (literal (literalValue 7))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( (parameterList (parameter p1 (type Int))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end constructor) (property property p1 as (type Int)) (functionDef (functionWithBody function (functionSignature asString ( ) as (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";
        
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoSuchProperty()
    {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        self.p = p1
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
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }


    [TestMethod]
    public void Fail_MissingSelfCausesCompileErrorDueToAssigningToParam()
    {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        p1 = p1
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