using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T59_Global
{
    #region Passes

    [TestMethod]
    public void Pass_DisambiguateConstantFromLocalVariable()
    {
        var code = @"#
constant a = 4
main
  var a = 3
  printLine(global.a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public const int a = 4;
}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    printLine(Globals.a);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    [TestMethod]
    public void Pass_DisambiguateConstantFromInstanceProperty()
    {
        var code = @"#
constant a = 4

main
    var f = Foo()
    printLine(f.prop())
    printLine(f.const())
end main

class Foo
    constructor()
        a = 3
    end constructor

    property a as Int

    function prop() as Int
        return a
    end function

    function const() as Int
        return global.a
    end function

    function as String() as String
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
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n");
    }


    [TestMethod]
    public void Pass_DisambiguateGlobalFunctionFromInstanceFunction()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.loc())
    printLine(f.glob())
end main

function bar() as Int
    return 4
end function

class Foo
    constructor()
    end constructor

    function loc() as Int
        return bar()
    end function

    function glob() as Int
        return global.bar()
    end function

    function bar() as Int
        return 3
    end function

    function as String() as String
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
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoSuchGlobal()
    {
        var code = @"#
constant b = 4
main
  var a = 3
  printLine(global.a)
end main
";

        var objectCode = @"";

        var parseTree = @"*";

var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NoSuchGlobalConstant()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.prop())
    printLine(f.const())
end main

class Foo
    constructor()
        a = 3
    end constructor

    property a as Int

    function prop() as Int
        return a
    end function

    function const() as Int
        return global.a
    end function

    function as String() as String
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
    public void Pass_NoSuchGlobalSubroutine()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.loc())
    printLine(f.glob())
end main

class Foo
    constructor()
    end constructor

    function loc() as Int
        return bar()
    end function

    function glob() as Int
        return global.bar()
    end function

    function bar() as Int
        return 3
    end function

    function as String() as String
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