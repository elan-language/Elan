using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T70_StandardHofs {
    #region Passes

    [TestMethod]
    public void Pass_Filter() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
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
    public void Pass_ParallelFilter() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.asParallel().filter(lambda x -> x > 20)
 print source.asParallel().filter(lambda x -> x > 20).asList()
 print source.asParallel().filter(lambda x -> x < 3 or x > 35).asList()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.filter(StandardLibrary.Functions.asParallel(source), (x) => x > 20)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.asList(StandardLibrary.Functions.filter(StandardLibrary.Functions.asParallel(source), (x) => x > 20))));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.asList(StandardLibrary.Functions.filter(StandardLibrary.Functions.asParallel(source), (x) => x < 3 || x > 35))));
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
    public void Pass_Map() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
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
    public void Pass_Reduce() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.reduce(0, lambda s,x -> s+x)
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.reduce(source, 0, (s, x) => s + x)));
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
    public void Pass_Max() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.max()
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
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "37\r\n");
    }

    [TestMethod]
    public void Pass_MaxBy() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.maxBy(lambda x -> x mod 5)
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.maxBy(source, (x) => x % 5)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "19\r\n");
    }

    [TestMethod]
    public void Pass_MaxBy1() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {{1}, {2,2}}
main
 print source.maxBy(lambda t -> t.count())
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<StandardLibrary.ElanList<int>> source = new StandardLibrary.ElanList<StandardLibrary.ElanList<int>>(new StandardLibrary.ElanList<int>(1), new StandardLibrary.ElanList<int>(2, 2));
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.maxBy(source, (t) => StandardLibrary.Functions.count(t))));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {2,2}\r\n");
    }

    [TestMethod]
    public void Pass_MaxBy2() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {""apple"", ""orange"",""pear""}
main
 print source.maxBy(lambda t -> t.length())
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<string> source = new StandardLibrary.ElanList<string>(@$""apple"", @$""orange"", @$""pear"");
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.maxBy(source, (t) => StandardLibrary.Functions.length(t))));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "orange\r\n");
    }

    [TestMethod]
    public void Pass_Count() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.count()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.count(source)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_Min() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.min()
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
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\n");
    }

    [TestMethod]
    public void Pass_MinBy() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.minBy(lambda x -> x mod 5)
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.minBy(source, (x) => x % 5)));
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
    public void Pass_Any() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
  print source.any(lambda x -> x > 20)
  print source.any(lambda x -> x mod 2 is 0)
  print source.any(lambda x -> x > 40)

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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.any(source, (x) => x > 20)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.any(source, (x) => x % 2 == 0)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.any(source, (x) => x > 40)));
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
    public void Pass_GroupBy() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var gs set to source.groupBy(lambda x -> x mod 5).asList()
  print gs
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
    var gs = StandardLibrary.Functions.asList(StandardLibrary.Functions.groupBy(source, (x) => x % 5));
    System.Console.WriteLine(StandardLibrary.Functions.asString(gs));
  }
}";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {Group key:0 count: 1,Group key:1 count: 2,Group key:2 count: 5,Group key:3 count: 3,Group key:4 count: 1}\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_MaxOnNonNumeric() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {""apple"",""orange"",""pear""}
main
 print source.max()
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<string> source = new StandardLibrary.ElanList<string>(@$""apple"", @$""orange"", @$""pear"");
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.max(source)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_MaxLambdaReturningNonNumeric() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant source set to {""apple"", ""orange"",""pear""}
main
 print source.maxBy(lambda t -> t.asUpperCase())
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<string> source = new StandardLibrary.ElanList<string>(@$""apple"", @$""orange"", @$""pear"");
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.maxBy(source, (t) => StandardLibrary.Functions.asUpperCase(t))));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}