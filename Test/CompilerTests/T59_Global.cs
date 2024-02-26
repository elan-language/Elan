using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T59_Global {
    #region Passes

    [TestMethod]
    public void Pass_DisambiguateConstantFromLocalVariable() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
constant a set to 4
main
  var a set to 3
  print global.a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public const int a = 4;
}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.a));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    [TestMethod]
    public void Pass_DisambiguateConstantFromInstanceProperty() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
constant a set to 4

main
    var f set to new Foo()
    print f.prop()
    print f.cons()
end main

class Foo
    constructor()
        set a to 3
    end constructor

    property a Int

    function prop() as Int
        return a
    end function

    function cons() as Int
        return global.a
    end function

    function asString() as String
        return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.prop()));
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.cons()));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n");
    }

    [TestMethod]
    public void Pass_DisambiguateGlobalFunctionFromInstanceFunction() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    print f.loc()
    print f.glob()
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

    function asString() as String
        return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.loc()));
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.glob()));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
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
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
constant b set to 4
main
  var a set to 3
  print global.a
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NoSuchGlobalConstant() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    print f.prop()
    print f.cons()
end main

class Foo
    constructor()
        set a to 3
    end constructor

    property a Int

    function prop() as Int
        return a
    end function

    function cons() as Int
        return global.a
    end function

    function asString() as String
        return """"
    end function

end class
";
        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NoSuchGlobalSubroutine() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    print f.loc()
    print f.glob()
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

    function asString() as String
        return """"
    end function

end class
";
        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}