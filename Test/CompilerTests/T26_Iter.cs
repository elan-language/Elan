using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T26_Iter {
    #region Passes

    [TestMethod]
    public void Pass_List() {
        var code = @"# Elanv0.1 Parsed FFFF
main
  var it set to { 1,5,6}
  call printEach(it)
end main

procedure printEach(target Iter<of Int>)
  each x in target
    print x
  end each
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printEach(System.Collections.Generic.IEnumerable<int> target) {
    foreach (var x in target) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var it = new StandardLibrary.ElanList<int>(1, 5, 6);
    Globals.printEach(it);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n5\r\n6\r\n");
    }

    [TestMethod]
    public void Pass_Array() {
        var code = @"# Elanv0.1 Parsed FFFF
main
  var it set to { 1,3,6}.asArray()
  call printEach(it)
end main

procedure printEach(target Iter<of Int>)
  each x in target
    print x
  end each
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printEach(System.Collections.Generic.IEnumerable<int> target) {
    foreach (var x in target) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var it = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(1, 3, 6));
    Globals.printEach(it);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n3\r\n6\r\n");
    }

    [TestMethod]
    public void Pass_String() {
        var code = @"# Elanv0.1 Parsed FFFF
main
  var s set to ""Foo""
  call printEach(s)
end main

procedure printEach(target Iter<of Char>)
  each x in target
    print x
  end each
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printEach(System.Collections.Generic.IEnumerable<char> target) {
    foreach (var x in target) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var s = @$""Foo"";
    Globals.printEach(s);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "F\r\no\r\no\r\n");
    }

    [TestMethod]
    public void Pass_Printing() {
        var code = @"# Elanv0.1 Parsed FFFF
main
  var it set to { 1,2,3,4,5,6,7}
  call printAsIter(it)
  call printAsList(it)
end main

procedure printAsIter(target Iter<of Int>)
  print target
end procedure

procedure printAsList(target Iter<of Int>)
  var some set to target.asList()[3..]
  print some
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printAsIter(System.Collections.Generic.IEnumerable<int> target) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(target));
  }
  public static void printAsList(System.Collections.Generic.IEnumerable<int> target) {
    var some = StandardLibrary.Functions.asList(target)[(3)..];
    System.Console.WriteLine(StandardLibrary.Functions.asString(some));
  }
}

public static class Program {
  private static void Main(string[] args) {
    var it = new StandardLibrary.ElanList<int>(1, 2, 3, 4, 5, 6, 7);
    Globals.printAsIter(it);
    Globals.printAsList(it);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {1,2,3,4,5,6,7}\r\nList {4,5,6,7}\r\n");
    }

    [TestMethod]
    public void Pass_Default() {
        var code = @"# Elanv0.1 Parsed FFFF
main
  var f set to new Foo()
  print f.it
end main

class Foo
  constructor()
  end constructor

  property it Iter<of Int>

  function asString() as String
    return ""A Foo""
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
    public virtual System.Collections.Generic.IEnumerable<int> it { get; set; } = _DefaultIter<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A Foo"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override System.Collections.Generic.IEnumerable<int> it => _DefaultIter<int>.DefaultInstance;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    System.Console.WriteLine(StandardLibrary.Functions.asString(f.it));
  }
}";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "empty iter\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoGenericTypeSpecified() {
        var code = @"# Elanv0.1 Parsed FFFF
main
end main

procedure printEach(target Iter)
  each x in target
    print x
  end each
end procedure
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_PassArgumentWithWrongGenericType() {
        var code = @"# Elanv0.1 Parsed FFFF
main
  var it set to {1,2,3,4,5,6,7}
  call printEach(it)
end main

procedure printEach(target Iter<of String>)
  each x in target
    print x
  end each
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_Indexing() {
        var code = @"# Elanv0.1 Parsed FFFF
main
  var it set to { 1,2,3,4,5,6,7}
  call printEach(it[2..4])
end main

procedure printEach(target Iter<of Int>)
  each x in target
    print x
  end each
  print target[0]
end procedure
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