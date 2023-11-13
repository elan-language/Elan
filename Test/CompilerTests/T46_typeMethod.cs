using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T46_typeMethod {
    #region Passes

    [TestMethod]
    public void Pass_Template() {
        var code = @"
main
    printLine(1.type())
    printLine(1.1.type())
    printLine('a'.type())
    printLine(false.type())
    printLine(""a"".type())
    printLine({1,2,3}.type())
    printLine({'a': 3}.type())
    printLine({1,2,3}.asArray().type())
    printLine(Foo(3).type())
end main

class Foo
    constructor(p1 Int)
        set self.p1 to p1 * 2
    end constructor

    property p1 Int

    function asString() -> String
        return ""{p1}""
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1 * 2;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string asString() {

      return @$""{p1}"";
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
    printLine(StandardLibrary.Functions.type(1));
    printLine(StandardLibrary.Functions.type(1.1));
    printLine(StandardLibrary.Functions.type('a'));
    printLine(StandardLibrary.Functions.type(false));
    printLine(StandardLibrary.Functions.type(@$""a""));
    printLine(StandardLibrary.Functions.type(new StandardLibrary.ElanList<int>(1, 2, 3)));
    printLine(StandardLibrary.Functions.type(new StandardLibrary.ElanDictionary<char,int>(('a', 3))));
    printLine(StandardLibrary.Functions.type(StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(1, 2, 3))));
    printLine(StandardLibrary.Functions.type(new Foo(3)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Int\r\nFloat\r\nChar\r\nBool\r\nString\r\nList<Int>\r\nDictionary<Char,Int>\r\nArray<Int>\r\nFoo\r\n");
    }

    [TestMethod]
    public void Pass_TypeTesting() {
        var code = @"
main
    printLine(1.type() is 2.type());
    printLine(1.0.type() is 2.type());
end main

class Foo
    constructor(p1 Int)
        set self.p1 to p1 * 2
    end constructor

    property p1 Int

    function asString() -> String
        return ""{p1}""
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p1) {
      this.p1 = p1 * 2;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string asString() {

      return @$""{p1}"";
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
    printLine(StandardLibrary.Functions.type(1) == StandardLibrary.Functions.type(2));
    printLine(StandardLibrary.Functions.type(1.0) == StandardLibrary.Functions.type(2));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    #endregion

    #region Fails

    #endregion
}