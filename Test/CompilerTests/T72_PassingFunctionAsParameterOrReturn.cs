using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T72_PassingFunctionAsParameterOrReturn {
    #region Passes

    [TestMethod]
    public void Pass_PassAsParam() {
        var code = @"
main
  call printModified(3, twice)
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure

function twice(x Int) as Int
  return x * 2
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printModified(int i, Func<int, int> f) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(f(i)));
  }
  public static int twice(int x) {

    return x * 2;
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.printModified(3, twice);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "6\r\n");
    }

    [TestMethod]
    public void Pass_PassAsParam2() {
        var code = @"
main
  call printIt(""Hello"", 'e', find)
end main

procedure printIt(s String, c Char, f (String, Char -> Int))
  print f(s,c)
end procedure

function find(x String, y Char ) as Int
  return x.indexOf(y)
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printIt(string s, char c, Func<string, char, int> f) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(f(s, c)));
  }
  public static int find(string x, char y) {

    return StandardLibrary.Functions.indexOf(x, y);
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.printIt(@$""Hello"", 'e', find);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_ReturnAFunction() {
        var code = @"
main
  var f = getFunc()
  print f(5)
end main

function getFunc() as (Int -> Int)
  return twice
end function

function twice(x Int) as Int
  return x * 2
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static Func<int, int> getFunc() {

    return twice;
  }
  public static int twice(int x) {

    return x * 2;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = Globals.getFunc();
    System.Console.WriteLine(StandardLibrary.Functions.asString(f(5)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_FuncAsProperty() {
        var code = @"
main
  var foo = new Foo(twice)
  print foo.f(7)
end main

class Foo
  constructor(f (Int -> Int))
    set self.f to f
  end constructor

  property f (Int -> Int)

  function asString() as String
    return ""a Foo""
  end function
end class

function twice(x Int) as Int
  return x * 2
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int twice(int x) {

    return x * 2;
  }
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(Func<int, int> f) {
      this.f = f;
    }
    public virtual Func<int, int> f { get; set; } = (_) => default;
    public virtual string asString() {

      return @$""a Foo"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override Func<int, int> f => (_) => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var foo = new Foo(twice);
    System.Console.WriteLine(StandardLibrary.Functions.asString(foo.f(7)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "14\r\n");
    }

    [TestMethod]
    public void Pass_DefaultValue() {
        var code = @"
main
  var foo = new Foo()
  print foo.f
  print foo.f(7)
end main

class Foo
  constructor()
  end constructor

  property f (Int -> Int)

  function asString() as String
    return ""a Foo""
  end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {

    }
    public virtual Func<int, int> f { get; set; } = (_) => default;
    public virtual string asString() {

      return @$""a Foo"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override Func<int, int> f => (_) => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var foo = new Foo();
    System.Console.WriteLine(StandardLibrary.Functions.asString(foo.f));
    System.Console.WriteLine(StandardLibrary.Functions.asString(foo.f(7)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Function (Int -> Int)\r\n0\r\n");
    }

    [TestMethod]
    public void Pass_DefaultValueObj() {
        var code = @"
main
  var foo = new Foo()
  print foo.f
  print foo.f(7)
end main

class Foo
  constructor()
  end constructor

  property f (Int -> Foo)

  function asString() as String
    return ""a Foo""
  end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {

    }
    public virtual Func<int, Foo> f { get; set; } = (_) => Foo.DefaultInstance;
    public virtual string asString() {

      return @$""a Foo"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override Func<int, Foo> f => (_) => Foo.DefaultInstance;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var foo = new Foo();
    System.Console.WriteLine(StandardLibrary.Functions.asString(foo.f));
    System.Console.WriteLine(StandardLibrary.Functions.asString(foo.f(7)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Function (Int -> Foo)\r\ndefault Foo\r\n");
    }

    [TestMethod]
    public void Pass_PrintingAFunction() {
        var code = @"
main
  print twice
end main

function twice(x Int) as Int
  return x * 2
end function
";
        var parseTree = @"*";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int twice(int x) {

    return x * 2;
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(twice));
  }
}";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Function (Int -> Int)\r\n");
    }


    #endregion

    #region Fails

    [TestMethod]
    public void Fail_FunctionSignatureDoesntMatch() {
        var code = @"
main
  call printModified(3, power)
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure

function power(x Int, y Int) as Int
  return x ^ y
end function
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_UsingReturnedFuncWithoutArgs() {
        var code = @"
main
  var a = getFunc()
  print a()
end main

function getFunc() as (Int -> Int)
  return twice
end function

function twice(x Int) as Int
  return x * 2
end function
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