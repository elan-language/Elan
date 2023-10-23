﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T56_PrivateProperties
{
    #region Passes

    [TestMethod]
    public void Pass_PrivatePropertyCanBeDeclared()
    {
        var code = @"#
main
    var x = Foo()
end main


class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String

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
      p2 = @$""Apple"";
    }
    public int p1 { get; set; }
    private string p2 { get; set; } = """";
    public string asString() {

      return @$"""";
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo();
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5))))) (assignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple"")))))) end constructor) (property property p1 as (type Int)) (property private property p2 as (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) as (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_PrivatePropertyCannotBeAccessed()
    {
        var code = @"#
main
    var f = Foo()
    var s = f.p2
end main

class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String

    function asString() as String
         return """"
    end function

end class
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}