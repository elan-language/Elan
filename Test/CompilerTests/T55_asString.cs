﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T55_asString {
    #region Fails

   

    #endregion

    #region Passes

    [TestMethod]
    public void Pass_ClassHasNoAsString() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    var s set to f.asString()
    print s
end main
class Foo
    constructor()
        set p1 to 5
        set p2 to ""Apple""
    end constructor

    property p1 Int

    private property p2 String
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
      p1 = 5;
      p2 = @$""Apple"";
    }
    public virtual int p1 { get; set; } = default;
    protected virtual string p2 { get; set; } = """";

    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      protected override string p2 => """";

      public string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    var s = StandardLibrary.Functions.asString(f);
    System.Console.WriteLine(StandardLibrary.Functions.asString(s));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "a Foo\r\n");
    }

    [TestMethod]
    public void Pass_DefaultClassAsString() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    var s set to f.p1.asString()
    print s
end main
class Foo
    constructor()
    end constructor

    property p1 Foo
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
    public virtual Foo p1 { get; set; } = Foo.DefaultInstance;

    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override Foo p1 => Foo.DefaultInstance;

      public string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    var s = StandardLibrary.Functions.asString(f.p1);
    System.Console.WriteLine(StandardLibrary.Functions.asString(s));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "default Foo\r\n");
    }

    [TestMethod]
    public void Pass_DefaultClassReplacesAsString() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    var s1 set to f.asString()
    var s2 set to f.p1.asString()
    print s1
    print s2
end main
class Foo
    constructor()
    end constructor

    property p1 Foo

    function asString() as String
         return ""Custom asString""
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
    public virtual Foo p1 { get; set; } = Foo.DefaultInstance;
    public virtual string asString() {

      return @$""Custom asString"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override Foo p1 => Foo.DefaultInstance;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    var s1 = f.asString();
    var s2 = f.p1.asString();
    System.Console.WriteLine(StandardLibrary.Functions.asString(s1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(s2));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Custom asString\r\ndefault Foo\r\n");
    }

    [TestMethod]
    public void Pass_AsStringMayBeCalled() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    var s set to f.asString()
    print s
end main
class Foo
    constructor()
        set p1 to 5
        set p2 to ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() as String
         return p2
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(s));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_AsStringCalledWhenObjectPrinted() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    print f
end main
class Foo
    constructor()
        set p1 to 5
        set p2 to ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() as String
         return p2
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(f));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_AsStringUsingDefaultHelper() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var f set to new Foo()
    print f
end main

class Foo
    constructor()
        set p1 to 5
        set p2 to ""Apple""
    end constructor

    property p1 Int

    private property p2 String

    function asString() as String
         return property.typeAndProperties()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(f));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Foo {p1:5, p2:Apple}\r\n");
    }

    [TestMethod]
    public void Pass_AsStringOnVariousDataTypes() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var l set to {1,2,3}
    var sl set to l.asString()
    print sl
    var a set to {1,2,3}.asArray()
    var sa set to a.asString()
    print sa
    var d set to {'a':1, 'b':3, 'z':10}
    var sd set to d.asString()
    print sd
end main

";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var l = new StandardLibrary.ElanList<int>(1, 2, 3);
    var sl = StandardLibrary.Functions.asString(l);
    System.Console.WriteLine(StandardLibrary.Functions.asString(sl));
    var a = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(1, 2, 3));
    var sa = StandardLibrary.Functions.asString(a);
    System.Console.WriteLine(StandardLibrary.Functions.asString(sa));
    var d = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
    var sd = StandardLibrary.Functions.asString(d);
    System.Console.WriteLine(StandardLibrary.Functions.asString(sd));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {1,2,3}\r\nArray {1,2,3}\r\nDictionary {a:1,b:3,z:10}\r\n");
    }

    #endregion
}