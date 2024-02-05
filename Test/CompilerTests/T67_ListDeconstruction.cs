using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T67_ListDeconstruction {
    #region Passes

    [TestMethod]
    public void Pass_IntoNewVarsList() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var {x:xs} set to source
  print x
  print xs
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<int> source = new StandardLibrary.ElanList<int>(2, 3, 5, 7, 11, 13, 17, 19, 23, 27, 31, 37);
}

public static class Program {
  private static void Main(string[] args) {
    var (x, xs) = source;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(xs));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nList {3,5,7,11,13,17,19,23,27,31,37}\r\n");
    }

    [TestMethod]
    public void Pass_IntoNewVarsArray() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var {x:xs} set to source.asArray()
  print x
  print xs
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<int> source = new StandardLibrary.ElanList<int>(2, 3, 5, 7, 11, 13, 17, 19, 23, 27, 31, 37);
}

public static class Program {
  private static void Main(string[] args) {
    var (x, xs) = StandardLibrary.Functions.asArray(source);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(xs));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nArray {3,5,7,11,13,17,19,23,27,31,37}\r\n");
    }

    [TestMethod]
    public void Pass_IntoNewVarsIter() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
  call p(source)
end main

procedure p(it Iter<of Int>)
  var {x:xs} set to it
  print x
  print xs
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<int> source = new StandardLibrary.ElanList<int>(2, 3, 5, 7, 11, 13, 17, 19, 23, 27, 31, 37);
  public static void p(System.Collections.Generic.IEnumerable<int> it) {
    var (x, xs) = it;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(xs));
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.p(source);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nIter {3,5,7,11,13,17,19,23,27,31,37}\r\n");
    }

    [TestMethod]
    public void Pass_IntoExistingVars() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var x set to 0
  var xs set to default List<of Int>
  set {x:xs} to source
  print x
  print xs
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<int> source = new StandardLibrary.ElanList<int>(2, 3, 5, 7, 11, 13, 17, 19, 23, 27, 31, 37);
}

public static class Program {
  private static void Main(string[] args) {
    var x = 0;
    var xs = StandardLibrary.ElanList<int>.DefaultInstance;
    (x, xs) = source;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(xs));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nList {3,5,7,11,13,17,19,23,27,31,37}\r\n");
    }

    #endregion

    #region Fails

    #endregion
}