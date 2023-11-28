using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T70_StandardHofs
{
    #region Passes

    [TestMethod]
    public void Pass_Filter()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.filter(lambda x -> x > 20)
 print source.filter(lambda x -> x > 20).asList()
 print source.filter(lambda x -> x < 3 or x > 35).asList()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.filter(source, (x) => x > 20)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.asList(StandardLibrary.Functions.filter(source, (x) => x > 20))));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.asList(StandardLibrary.Functions.filter(source, (x) => x < 3 || x > 35))));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Iter {23,27,31,37}\r\nList {23,27,31,37}\r\nList {2,37}\r\n");
    }

    [TestMethod]
    public void Pass_Map()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.map(lambda x -> x + 1).asList()
 print source.map(lambda x -> x.asString() + '*').asList()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.asList(StandardLibrary.Functions.map(source, (x) => x + 1))));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.asList(StandardLibrary.Functions.map(source, (x) => StandardLibrary.Functions.asString(x) + '*'))));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {3,4,6,8,12,14,18,20,24,28,32,38}\r\nList {2*,3*,5*,7*,11*,13*,17*,19*,23*,27*,31*,37*}\r\n");
    }

    [TestMethod]
    public void Pass_Reduce()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.reduce(lambda s,x -> s+x)
 print source.reduce(100, lambda s,x -> s+x)
 print source.reduce(""Concat:"",lambda s,x -> s+x.asString())
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.reduce(source, (s, x) => s + x)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.reduce(source, 100, (s, x) => s + x)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.reduce(source, @$""Concat:"", (s, x) => s + StandardLibrary.Functions.asString(x))));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "195\r\n295\r\nConcat:23571113171923273137\r\n");
    }
    [TestMethod]
    public void Pass_Max()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.max()
 print source.max(lambda x -> x mod 5)
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.max(source)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.max(source, (x) => x % 5)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "37\r\n19\r\n");
    }
    [TestMethod]
    public void Pass_Min()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.min()
 print source.min(lambda x -> x mod 5)
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.min(source)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.min(source, (x) => x % 5)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\n5\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_Any()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
  print source.any(lambda x -> x > 20))
  print source.any(lambda x -> x mod 2 is 0))
  print source.any(lambda x -> x > 40))

end main
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrye\r\nfalse");
    }

    [TestMethod, Ignore]
    public void Pass_GroupBy()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var gs = source.groupBy(lambda x -> x mod 5)) # {5} {11,31} {2,7,17,27,37} {13,23,19}
  print gs
  print gs[2]
end main
";
        var objectCode = @"";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Iter<Group<Int,Int>>\r\nGroup 1 {11,31}"); //Not sure quite what output is reasonable?
    }

    #endregion

    #region Fails

    #endregion
}