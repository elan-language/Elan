using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T17_Dictionaries {
    #region Passes

    [TestMethod]
    public void Pass_LiteralConstantAndPrinting() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(a);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_AccessByKey() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a['z'])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(a['z']);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_keys() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a.keys())
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(StandardLibrary.Functions.keys(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {a,b,z}\r\n");
    }

    [TestMethod]
    public void Pass_hasKey() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a.hasKey('b'))
  printLine(a.hasKey('d'))
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(StandardLibrary.Functions.hasKey(a, 'b'));
    printLine(StandardLibrary.Functions.hasKey(a, 'd'));
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
    public void Pass_values() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a.values())
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(StandardLibrary.Functions.values(a));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {1,3,10}\r\n");
    }

    [TestMethod]
    public void Pass_Set() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.set('b', 4)
  var c = b.set('d', 2)
  printLine(a)
  printLine(c)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    var b = StandardLibrary.Functions.set(a, 'b', 4);
    var c = StandardLibrary.Functions.set(b, 'd', 2);
    printLine(a);
    printLine(c);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\nDictionary {a:1,b:4,d:2,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_RemoveEntry() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.remove('b')
  printLine(a)
  printLine(b)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    var b = StandardLibrary.Functions.remove(a, 'b');
    printLine(a);
    printLine(b);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\nDictionary {a:1,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_RemoveInvalidKey() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.remove('c')
  printLine(b)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    var b = StandardLibrary.Functions.remove(a, 'c');
    printLine(b);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_CreateEmptyDictionary() {
        var code = @"#
main
  var a = Dictionary<String, Int>()
  var b = a.set(""Foo"",1)
  b = b.set(""Bar"", 3)
  print(b.length())
  print(b[""Foo""])
  print(b[""Bar""])
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
    var a = new StandardLibrary.ElanDictionary<string, int>();
    var b = StandardLibrary.Functions.set(a, @$""Foo"", 1);
    b = StandardLibrary.Functions.set(b, @$""Bar"", 3);
    print(StandardLibrary.Functions.length(b));
    print(b[@$""Foo""]);
    print(b[@$""Bar""]);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "213");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_RepeatedKey() {
        var code = @"#
constant a = {'a':1, 'b':3, 'a':10}
main
  printLine(a)
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "An element with the same key but a different value already exists. Key: 'a'");
    }

    [TestMethod]
    public void Fail_InconsistentTypes1() {
        var code = @"#
constant a = {'a':1, 'b':3.1, 'z':10}
main
  printLine(a)
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InconsistentTypes2() {
        var code = @"#
constant a = {'a':1, 'b':3.1, ""Z"":10}
main
  printLine(a)
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AccessByInvalidKey() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a['c'])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(a['c']);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. System.Collections.Generic.KeyNotFoundException"); //Messaging saying key does not exist or similar
    }

    [TestMethod]
    public void Fail_RemoveInvalidKeyType() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.remove(""b"")
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SetInvalidKeyType() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.set(""b"", 4)
end main
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