using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T26_Iter {
    #region Passes

    [TestMethod]
    public void Pass_List() {
        var code = @"
main
  var it = { 1,5,6}
  call printEach(it)
end main

procedure printEach(target Iter<of Int>)
  foreach x in target
    print x
  end foreach
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printEach(StandardLibrary.ElanIter<int> target) {
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
        var code = @"
main
  var it = { 1,3,6}.asArray()
  call printEach(it)
end main

procedure printEach(target Iter<of Int>)
  foreach x in target
    print x
  end foreach
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printEach(StandardLibrary.ElanIter<int> target) {
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

    [TestMethod, Ignore]
    public void Pass_String() {
        var code = @"
main
  var s = ""Foo""
  call printEach(s)
end main

procedure printEach(target Iter<of Char>)
  foreach x in target
    print x
  end foreach
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printEach(StandardLibrary.ElanIter<char> target) {
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
    public void Pass_Indexing() {
        var code = @"
main
  var it = { 1,2,3,4,5,6,7}
  call printEach(it[2..4])
end main

procedure printEach(target Iter<of Int>)
  foreach x in target
    print x
  end foreach
  print target[0]
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printEach(StandardLibrary.ElanIter<int> target) {
    foreach (var x in target) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(target[0]));
  }
}

public static class Program {
  private static void Main(string[] args) {
    var it = new StandardLibrary.ElanList<int>(1, 2, 3, 4, 5, 6, 7);
    Globals.printEach(it[(2)..(4)]);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n3\r\n");
    }

    [TestMethod]
    public void Pass_Printing() {
        var code = @"
main
  var it = { 1,2,3,4,5,6,7}
  call printAsIter(it)
  call printAsList(it)
end main

procedure printAsIter(target Iter<of Int>)
  var some = target[1..3]
  print some
end procedure

procedure printAsList(target Iter<of Int>)
  var some = target[3..].asList()
  print some
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printAsIter(StandardLibrary.ElanIter<int> target) {
    var some = target[(1)..(3)];
    System.Console.WriteLine(StandardLibrary.Functions.asString(some));
  }
  public static void printAsList(StandardLibrary.ElanIter<int> target) {
    var some = StandardLibrary.Functions.asList(target[(3)..]);
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
        AssertObjectCodeExecutes(compileData, "List {2,3}\r\nList {4,5,6,7}\r\n");
    }

    [TestMethod]
    public void Pass_Default() {
        var code = @"
main
  var f = new Foo()
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
    public virtual StandardLibrary.ElanIter<int> it { get; set; } = StandardLibrary.ElanIter<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A Foo"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override StandardLibrary.ElanIter<int> it => StandardLibrary.ElanIter<int>.DefaultInstance;

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
        var code = @"
main
end main

procedure printEach(target Iter)
  foreach x in target
    print x
  end foreach
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_PassArgumentWithWrongGenericType() {
        var code = @"
main
  var it = {1,2,3,4,5,6,7}
  call printEach(it)
end main

procedure printEach(target Iter<of String>)
  foreach x in target
    print x
  end foreach
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