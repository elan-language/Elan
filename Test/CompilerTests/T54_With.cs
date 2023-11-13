using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T54_With {
    #region Fails

    [TestMethod]
    public void Fail_NonMatchingProperty() {
        var code = @"#
main
    var x = Foo() with {p1 = 3, p3 = ""Apple"" }
    print x.p1
    print x.p2
end main

class Foo
    constructor()
        set p1 to 5
    end constructor
    property p1 Int

    property p2 String

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

    #region Passes

    [TestMethod]
    public void Pass_InstantiatingClassWithZeroParamConstructor() {
        var code = @"#
main
    var x = Foo() with {p1 = 3, p2 = ""Apple"" }
    print x.p1
    print x.p2
end main

class Foo
    constructor()
        set p1 to 5
    end constructor
    property p1 Int

    property p2 String

    function asString() -> String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo() with {p1 = 3, p2 = @$""Apple""};
    print(x.p1);
    print(x.p2);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_ConstructorWithParm() {
        var code = @"#
main
    var x = Foo(7) with {p1 = 3, p2 = ""Apple"" }
    print x.p1
    print x.p2
end main

class Foo
    constructor(p1 Int)
        set self.p1 to p1
    end constructor
    property p1 Int

    property p2 String

    function asString() -> String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo(7) with {p1 = 3, p2 = @$""Apple""};
    print(x.p1);
    print(x.p2);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_AppliedToInstanceButReturnedOneIsNewInstance() {
        var code = @"#
main
    var x = Foo()
    var y = x with {p1 = 3, p2 = ""Apple"" }
    print y.p1
    print y.p2
    print x.p1
end main

class Foo
    constructor()
        set p1 to 5
    end constructor
    property p1 Int

    property p2 String

    function asString() -> String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo();
    var y = x with {p1 = 3, p2 = @$""Apple""};
    print(y.p1);
    print(y.p2);
    print(x.p1);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n5\r\n");
    }

    [TestMethod]
    public void Pass_WorksWithImmutableClass() {
        var code = @"#
main
    var x = Foo()
    var y = x with {p1 = 3, p2 = ""Apple"" }
    print y.p1
    print y.p2
    print x.p1
end main

immutable class Foo
    constructor()
        set p1 to 5
    end constructor
    property p1 Int

    property p2 String

    function asString() -> String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; init; } = default;
    public virtual string p2 { get; init; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo();
    var y = x with {p1 = 3, p2 = @$""Apple""};
    print(y.p1);
    print(y.p2);
    print(x.p1);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n5\r\n");
    }

    #endregion
}