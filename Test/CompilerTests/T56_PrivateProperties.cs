using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T56_PrivateProperties {
    #region Passes

    [TestMethod]
    public void Pass_PrivatePropertyCanBeDeclared() {
        var code = @"#
main
    var x = Foo()
end main


class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String

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
      p2 = @$""Apple"";
    }
    public virtual int p1 { get; set; } = default;
    protected virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
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
    var x = new Foo();
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Foo) ( ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 5))))) (assignment (assignableValue p2) = (expression (value (literal (literalDataStructure ""Apple"")))))) end constructor) (property property p1 (type Int)) (property private property p2 (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

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
    public void Fail_PrivatePropertyCannotBeAccessed() {
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

    property p1 Int

    private property p2 String

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

    #endregion
}