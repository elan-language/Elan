using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T14_Lists {
    #region Passes

    [TestMethod]
    public void Pass_literalList() {
        var code = @"
main
    var a = {4,5,6,7,8}
    print a
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
    var a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4,5,6,7,8}\r\n");
    }

    [TestMethod]
    public void Pass_literalListOfClass() {
        var code = @"
main
    var a = Foo()
    var b = {a}
    print b
end main

class Foo
  constructor()
  end constructor

  function asString() -> String
    return ""foo""
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

    }

    public virtual string asString() {

      return @$""foo"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }


      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = new Foo();
    var b = new StandardLibrary.ElanList<Foo>(a);
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {foo}\r\n");
    }

    [TestMethod]
    public void Pass_literalListOfValueId() {
        var code = @"
main
    var a = 1
    var b = 1.1
    var c = 'c'
    var d = ""d""
    var e = true
    var v = {a}
    var w = {b}
    var x = {c}
    var y = {d}
    var z = {e}
    print v
    print w
    print x
    print y
    print z
end main

class Foo
  constructor()
  end constructor

  function asString() -> String
    return ""foo""
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

    }

    public virtual string asString() {

      return @$""foo"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }


      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 1;
    var b = 1.1;
    var c = 'c';
    var d = @$""d"";
    var e = true;
    var v = new StandardLibrary.ElanList<int>(a);
    var w = new StandardLibrary.ElanList<double>(b);
    var x = new StandardLibrary.ElanList<char>(c);
    var y = new StandardLibrary.ElanList<string>(d);
    var z = new StandardLibrary.ElanList<bool>(e);
    System.Console.WriteLine(StandardLibrary.Functions.asString(v));
    System.Console.WriteLine(StandardLibrary.Functions.asString(w));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(z));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {1}\r\nList {1.1}\r\nList {c}\r\nList {d}\r\nList {true}\r\n");
    }

    [TestMethod]
    public void Pass_literalListOfString() {
        var code = @"
main
    var a = {""Foo"", ""Bar""}
    print a
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
    var a = new StandardLibrary.ElanList<string>(@$""Foo"", @$""Bar"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {Foo,Bar}\r\n");
    }

    [TestMethod]
    public void Pass_literalListWithCoercion() {
        var code = @"
main
    var a = {4.1,5,6,7,8}
    print a
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
    var a = new StandardLibrary.ElanList<double>(4.1, 5, 6, 7, 8);
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4.1,5,6,7,8}\r\n");
    }

    [TestMethod]
    public void Pass_length() {
        var code = @"
main
    var a = {4,5,6,7,8}
    print a.length()
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
    var a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.length(a)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n");
    }

    [TestMethod]
    public void Pass_emptyList() {
        var code = @"
main
    var a = List<Int>()
    print a.length()
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
    var a = new StandardLibrary.ElanList<int>();
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.length(a)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0\r\n");
    }

    [TestMethod]
    public void Pass_index() {
        var code = @"
main
    var a = {4,5,6,7,8}
    print a[2]
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
    var a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[2]));
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
    public void Pass_range() {
        var code = @"
main
    var a = {4,5,6,7,8}
    print a[2..]
    print a[1..3]
    print a[..2]
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
    var a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[(2)..]));
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[(1)..(3)]));
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[..(2)]));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {6,7,8}\r\nList {5,6}\r\nList {4,5}\r\n");
    }

    [TestMethod]
    public void Pass_addElementToList() {
        var code = @"
main
    var a = {4,5,6,7,8}
    var b = a + 9
    print a
    print b
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
    var a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
    var b = a + 9;
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4,5,6,7,8}\r\nList {4,5,6,7,8,9}\r\n");
    }

    [TestMethod]
    public void Pass_addListToElement() {
        var code = @"
main
    var a = {4,5,6,7,8}
    var b = 9 + a
    print a
    print b
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
    var a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
    var b = 9 + a;
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4,5,6,7,8}\r\nList {9,4,5,6,7,8}\r\n");
    }

    [TestMethod]
    public void Pass_addListToListUsingPlus() {
        var code = @"
main
    var a = {4,5,6,7,8}
    var b = {1,2,3}
    var c = a + b
    print c
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
    var a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
    var b = new StandardLibrary.ElanList<int>(1, 2, 3);
    var c = a + b;
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4,5,6,7,8,1,2,3}\r\n");
    }

    [TestMethod]
    public void Pass_constantLists() {
        var code = @"
constant a = {4,5,6,7,8}
main
    print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<int> a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4,5,6,7,8}\r\n");
    }

    [TestMethod]
    public void Pass_createEmptyListUsingConstructor() {
        var code = @"
main
    var a = List<Int>()
    print a
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
    var a = new StandardLibrary.ElanList<int>();
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "empty list\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_emptyLiteralList() {
        var code = @"
main
    var a = {}
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_literalListInconsistentTypes1() {
        var code = @"
main
    var a = {3, ""apples""}
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_literalListInconsistentTypes2() {
        var code = @"
main
    var a = {3, 3.1}
end main
"; //Because list type is decided by FIRST element

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_OutOfRange() {
        var code = @"
main
    var a = {4,5,6,7,8}
    var b = a[5];
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
    var a = new StandardLibrary.ElanList<int>(4, 5, 6, 7, 8);
    var b = a[5];
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Specified argument was out of the range of valid values");
    }

    #endregion
}