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
    print 1.type()
    print 1.1.type()
    print 'a'.type()
    print false.type()
    print ""a"".type()
    print {1,2,3}.type()
    print {'a': 3}.type()
    print {1,2,3}.asArray().type()
    print new Foo(3).type()
end main

class Foo
    constructor(p1 Int)
        set self.p1 to p1 * 2
    end constructor

    property p1 Int

    function asString() as String
        return ""{p1}""
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(1)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(1.1)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type('a')));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(false)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(@$""a"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(new StandardLibrary.ElanList<int>(1, 2, 3))));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(new StandardLibrary.ElanDictionary<char,int>(('a', 3)))));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(1, 2, 3)))));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(new Foo(3))));
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
    print 1.type() is 2.type();
    print 1.0.type() is 2.type();
end main

class Foo
    constructor(p1 Int)
        set self.p1 to p1 * 2
    end constructor

    property p1 Int

    function asString() as String
        return ""{p1}""
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(1) == StandardLibrary.Functions.type(2)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.type(1.0) == StandardLibrary.Functions.type(2)));
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