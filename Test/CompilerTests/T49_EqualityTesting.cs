using Compiler;
using CSharpLanguageModel;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T49_EqualityTesting {

    [TestInitialize]
    public void TestInit() {
        CodeHelpers.ResetUniqueId();
    }

    #region Passes

    [TestMethod]
    public void Pass_DifferentInstancesWithSameValuesAreEqual() {
        var code = @"#
main
    var x = Foo(7, ""Apple"")
    var y = Foo(7, ""Orange"")
    var z = Foo(7, ""Orange"")
    print x is x
    print x is y
    print y is z
end main

class Foo
    constructor(p1 Int, p2 String)
        set self.p1 to p1
        set self.p2 to p2
    end constructor
    property p1 Int
    property p2 String

    procedure setP1(v Int)
        set p1 to v
    end procedure

    function asString() -> String
      return ""{p1} {p2}""
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
    public Foo(int p1, string p2) {
      this.p1 = p1;
      this.p2 = p2;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$""{p1} {p2}"";
    }
    public virtual void setP1(ref int v) {
      p1 = v;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";
      public override void setP1(ref int v) { }
      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo(7, @$""Apple"");
    var y = new Foo(7, @$""Orange"");
    var z = new Foo(7, @$""Orange"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y == z));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_ActuallyTheSameReference() {
        var code = @"#
main
    var x = Foo(7, ""Apple"")
    var y = x
    call y.setP1(3)
    var z = Foo(8, ""Orange"")
    print x is x
    print x is y
    print x is z
end main

class Foo
    constructor(p1 Int, p2 String)
        set self.p1 to p1
        set self.p2 to p2
    end constructor

    property p1 Int
    property p2 String

    procedure setP1(v Int)
        set p1 to v
    end procedure

    function asString() -> String
      return ""{p1} {p2}""
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
    public Foo(int p1, string p2) {
      this.p1 = p1;
      this.p2 = p2;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$""{p1} {p2}"";
    }
    public virtual void setP1(ref int v) {
      p1 = v;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";
      public override void setP1(ref int v) { }
      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo(7, @$""Apple"");
    var y = x;
    var _setP1_1_0 = 3;
    y.setP1(ref _setP1_1_0);
    var z = new Foo(8, @$""Orange"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == z));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_StringEquality() {
        var code = @"#
main
    var x = ""Apple""
    var y = ""Apple""
    var z = ""apple""
    print x is y
    print x is z
end main
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = @$""Apple"";
    var y = @$""Apple"";
    var z = @$""apple"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == z));
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

    [TestMethod]
    public void Pass_ListEquality() {
        var code = @"#
main
    var x = {3,4,5}
    var y = {3,4,5}
    var z = {4,3,5}
    print x is y
    print x is z
end main
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = new StandardLibrary.ElanList<int>(3, 4, 5);
    var y = new StandardLibrary.ElanList<int>(3, 4, 5);
    var z = new StandardLibrary.ElanList<int>(4, 3, 5);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == z));
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

    [TestMethod]
    public void Pass_ArrayEquality() {
        var code = @"#
main
    var x = {3,4,5}.asArray()
    var y = {3,4,5}.asArray()
    var z = {4,3,5}.asArray()
    var w = {3,4,5}
    print x is y
    print x is z
    print x is w
end main
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(3, 4, 5));
    var y = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(3, 4, 5));
    var z = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(4, 3, 5));
    var w = new StandardLibrary.ElanList<int>(3, 4, 5);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == z));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == w));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_DictionaryEquality() {
        var code = @"#
main
    var x = {'a':3,'b':4,'c':5}
    var y = {'a':3,'b':4,'c':5}
    var z = {'b':4,'c':5,'a':3}
    var w = {'a':3,'b':6,'c':5}
    var v = {""b"":4,""c"":5,""a"":3}
    print x is y
    print x is z
    print x is w
    print x is v
end main
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = new StandardLibrary.ElanDictionary<char,int>(('a', 3), ('b', 4), ('c', 5));
    var y = new StandardLibrary.ElanDictionary<char,int>(('a', 3), ('b', 4), ('c', 5));
    var z = new StandardLibrary.ElanDictionary<char,int>(('b', 4), ('c', 5), ('a', 3));
    var w = new StandardLibrary.ElanDictionary<char,int>(('a', 3), ('b', 6), ('c', 5));
    var v = new StandardLibrary.ElanDictionary<string,int>((@$""b"", 4), (@$""c"", 5), (@$""a"", 3));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == z));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == w));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == v));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\nfalse\r\n");
    }

    #endregion

    #region Fails

    #endregion
}