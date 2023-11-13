﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T55_asString {
    #region Fails

    [TestMethod]
    public void Fail_ClassHasNoAsString() {
        var code = @"#
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String
end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Class must have asString method");
    }

    #endregion

    #region Passes

    [TestMethod]
    public void Pass_AsStringMayBeCalled() {
        var code = @"#
main
    var f = Foo()
    var s = f.asString()
    printLine(s)
end main
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() -> String
         return p2
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

    public Foo() {
      p1 = 5;
      p2 = @$""Apple"";
    }
    public virtual int p1 { get; set; } = default;
    protected virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return p2;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      protected override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    var s = f.asString();
    printLine(s);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_AsStringCalledWhenObjectPrinted() {
        var code = @"#
main
    var f = Foo()
    printLine(f)
end main
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() -> String
         return p2
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

    public Foo() {
      p1 = 5;
      p2 = @$""Apple"";
    }
    public virtual int p1 { get; set; } = default;
    protected virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return p2;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      protected override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_AsStringUsingDefaultHelper() {
        var code = @"#
main
    var f = Foo()
    printLine(f)
end main

class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() -> String
         return self.typeAndProperties()
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

    public Foo() {
      p1 = 5;
      p2 = @$""Apple"";
    }
    public virtual int p1 { get; set; } = default;
    protected virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return StandardLibrary.Functions.typeAndProperties(this);
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      protected override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Foo {p1:5, p2:Apple}\r\n");
    }

    [TestMethod]
    public void Pass_AsStringOnVariousDataTypes() {
        var code = @"#
main
    var l = {1,2,3}
    var sl = l.asString()
    printLine(sl)
    var a = {1,2,3}.asArray()
    var sa = a.asString()
    printLine(sa)
    var d = {'a':1, 'b':3, 'z':10}
    var sd = d.asString()
    printLine(sd)
end main

";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var l = new StandardLibrary.ElanList<int>(1, 2, 3);
    var sl = StandardLibrary.Functions.asString(l);
    printLine(sl);
    var a = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(1, 2, 3));
    var sa = StandardLibrary.Functions.asString(a);
    printLine(sa);
    var d = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
    var sd = StandardLibrary.Functions.asString(d);
    printLine(sd);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {1,2,3}\r\nArray {1,2,3}\r\nDictionary {a:1,b:3,z:10}\r\n");
    }

    #endregion
}