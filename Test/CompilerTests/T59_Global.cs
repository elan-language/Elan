using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T59_Global {
    #region Passes

    [TestMethod]
    public void Pass_DisambiguateConstantFromLocalVariable() {
        var code = @"#
constant a = 4
main
  var a = 3
  call printLine(global.a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
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
    public void Pass_DisambiguateConstantFromInstanceProperty() {
        var code = @"#
constant a = 4

main
    var f = Foo()
    call printLine(f.prop())
    call printLine(f.cons())
end main

class Foo
    constructor()
        set a to 3
    end constructor

    property a Int

    function prop() -> Int
        return a
    end function

    function cons() -> Int
        return global.a
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
using static StandardLibrary.Constants;

public static partial class Globals {
  public const int a = 4;
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      a = 3;
    }
    public virtual int a { get; set; } = default;
    public virtual int prop() {

      return a;
    }
    public virtual int cons() {

      return Globals.a;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int a => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f.prop());
    printLine(f.cons());
  }
}";

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
    public void Pass_DisambiguateGlobalFunctionFromInstanceFunction() {
        var code = @"#
main
    var f = Foo()
    call printLine(f.loc())
    call printLine(f.glob())
end main

function bar() -> Int
    return 4
end function

class Foo
    constructor()
    end constructor

    function loc() -> Int
        return bar()
    end function

    function glob() -> Int
        return global.bar()
    end function

    function bar() -> Int
        return 3
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
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int bar() {

    return 4;
  }
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {

    }

    public virtual int loc() {

      return bar();
    }
    public virtual int glob() {

      return Globals.bar();
    }
    public virtual int bar() {

      return 3;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }


      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f.loc());
    printLine(f.glob());
  }
}";

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
    public void Fail_NoSuchGlobal() {
        var code = @"#
constant b = 4
main
  var a = 3
  call printLine(global.a)
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NoSuchGlobalConstant() {
        var code = @"#
main
    var f = Foo()
    call printLine(f.prop())
    call printLine(f.cons())
end main

class Foo
    constructor()
        set a to 3
    end constructor

    property a Int

    function prop() -> Int
        return a
    end function

    function cons() -> Int
        return global.a
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
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NoSuchGlobalSubroutine() {
        var code = @"#
main
    var f = Foo()
    call printLine(f.loc())
    call printLine(f.glob())
end main

class Foo
    constructor()
    end constructor

    function loc() -> Int
        return bar()
    end function

    function glob() -> Int
        return global.bar()
    end function

    function bar() -> Int
        return 3
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
        AssertDoesNotCompile(compileData, "Calling unknown method");
    }

    #endregion
}