using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T51_ProcedureMethods
{
    #region Passes

    [TestMethod, Ignore]
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
    public Foo() {
      p1 = 5;
    }
    public int p1 { get; set; }
    public string asString() {

      return @$"""";
    }
    public void setP1(ref int value) {
      p1 = value;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f.p1);
    printLine(f.p1);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n7\r\n");
    }

    [TestMethod, Ignore]
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
         return p""""
    end function

end class
";

        var objectCode = @"";

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

    [TestMethod, Ignore]
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