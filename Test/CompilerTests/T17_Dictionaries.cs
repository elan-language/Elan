using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T17_Dictionaries {
    #region Passes

    [TestMethod]
    public void Pass_LiteralConstantAndPrinting() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_AccessByKey() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  print a['z']
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a['z']));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_keys() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  print a.keys()
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.keys(a)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {a,b,z}\r\n");
    }

    [TestMethod]
    public void Pass_hasKey() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  print a.hasKey('b')
  print a.hasKey('d')
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.hasKey(a, 'b')));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.hasKey(a, 'd')));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_values() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  print a.values()
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.values(a)));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {1,3,10}\r\n");
    }

    [TestMethod]
    public void Pass_Set() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  var b set to a.setItem('b', 4)
  var c set to b.setItem('d', 2)
  print a
  print c
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    var b = StandardLibrary.Functions.setItem(a, 'b', 4);
    var c = StandardLibrary.Functions.setItem(b, 'd', 2);
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\nDictionary {a:1,b:4,d:2,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_RemoveEntry() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  var b set to a.removeItem('b')
  print a
  print b
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    var b = StandardLibrary.Functions.removeItem(a, 'b');
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\nDictionary {a:1,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_RemoveInvalidKey() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  var b set to a.removeItem('c')
  print b
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    var b = StandardLibrary.Functions.removeItem(a, 'c');
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_CreateEmptyDictionary() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var a set to new Dictionary<of String, Int>()
  var b set to a.setItem(""Foo"",1)
  set b to b.setItem(""Bar"", 3)
  print b.length()
  print b[""Foo""]
  print b[""Bar""]
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
    var a = new StandardLibrary.ElanDictionary<string, int>();
    var b = StandardLibrary.Functions.setItem(a, @$""Foo"", 1);
    b = StandardLibrary.Functions.setItem(b, @$""Bar"", 3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.length(b)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b[@$""Foo""]));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b[@$""Bar""]));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\n1\r\n3\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_RepeatedKey() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'a':10}
main
  print a
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "An element with the same key but a different value already exists. Key: 'a'");
    }

    [TestMethod]
    public void Fail_InconsistentTypes1() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3.1, 'z':10}
main
  print a
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InconsistentTypes2() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3.1, ""Z"":10}
main
  print a
end main
";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AccessByInvalidKey() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  print a['c']
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a['c']));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. System.Collections.Generic.KeyNotFoundException"); //Messaging saying key does not exist or similar
    }

    [TestMethod]
    public void Fail_RemoveInvalidKeyType() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  var b set to a.removeItem(""b"")
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SetInvalidKeyType() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant a set to {'a':1, 'b':3, 'z':10}
main
  var b set to a.setItem(""b"", 4)
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}