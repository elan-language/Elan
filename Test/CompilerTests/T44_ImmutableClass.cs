using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T44_ImmutableClass {
    #region Passes

    [TestMethod]
    public void Pass_BasicImmutableClass() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var f set to new Foo(3)
    print f.p1
    print f.square()
end main

immutable class Foo
    constructor(p1 Int)
        set property.p1 to p1
    end constructor
    property p1 Int
    function square() as Int
        return p1 * p1
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
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1;
    }
    public virtual int p1 { get; init; } = default;
    public virtual int square() {

      return p1 * p1;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo(3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.p1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.square()));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n9\r\n");
    }

    [TestMethod]
    public void Pass_AbstractImmutableClass() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var f set to new Foo(3)
    print f.p1
    print f.square()
end main

abstract immutable class Bar
    property p1 Int
    function square() as Int
end class

immutable class Foo inherits Bar
    constructor(p1 Int)
        set property.p1 to p1
    end constructor
    property p1 Int
    function square() as Int
        return p1 * p1
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
  public interface Bar {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();
    public int p1 { get; }
    public int square();
    private record class _DefaultBar : Bar {
      public _DefaultBar() { }
      public int p1 => default;
      public int square() => default;
      public string asString() { return ""default Bar"";  }
    }
  }
  public record class Foo : Bar {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1;
    }
    public virtual int p1 { get; init; } = default;
    public virtual int square() {

      return p1 * p1;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo(3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.p1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.square()));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n9\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_ProcedureMethod() {
        var code = @"# Elanv0.1 Parsed FFFF
immutable class Foo
    constructor(p1 Int)
        set property.p1 to p1
    end constructor

    property p1 Int

    procedure setP1(p1 Int)
        set property.p1 to p1
    end procedure
    
    function asString() as String
        return """"
    end function
end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_ProcedureMethodOnAbstractImmutableClass() {
        var code = @"# Elanv0.1 Parsed FFFF
abstract immutable class Bar
    property p1 Int

    procedure setP1(v Int)
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_AbstractAndImmutableReversed() {
        var code = @"# Elanv0.1 Parsed FFFF
immutable abstract class Bar
    property p1 Int

    procedure setP1(v Int)
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}