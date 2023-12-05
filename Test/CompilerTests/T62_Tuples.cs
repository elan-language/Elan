﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T62_Tuples {
    #region Passes

    [TestMethod]
    public void Pass_CreatingTuplesAndReadingContents() {
        var code = @"#
main
    var x set to (3,""Apple"")
    print x
    print x[0]
    print x[1]
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
    var x = (3, @$""Apple"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.Item1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.Item2));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(3, Apple)\r\n3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_FunctionReturnsTuple() {
        var code = @"#
main
    var x set to f()
    print x
    print x[0]
    print x[1]
end main
function f() as (String, String)
   return (""1"", ""2"")
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static (string, string) f() {

    return (@$""1"", @$""2"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = Globals.f();
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.Item1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.Item2));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(1, 2)\r\n1\r\n2\r\n");
    }

    [TestMethod]
    public void Pass_IndexFunctionReturnsTuple() {
        var code = @"#
main
    print f()[0]
end main
function f() as (String, String)
   return (""1"", ""2"")
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static (string, string) f() {

    return (@$""1"", @$""2"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.f().Item1));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_IndexGenericFunctionReturnsTuple() {
        var code = @"#
main
    print a.reduce((1,1), lambda i, j -> j)[0]
end main
constant a set to {(1,2)}
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<(int, int)> a = new StandardLibrary.ElanList<(int, int)>((1, 2));
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.reduce(a, (1, 1), (i, j) => j).Item1));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_FunctionTupleParameter() {
        var code = @"#
main
    var x set to ""one""
    var y set to ""two""
    print f((x,y))
end main
function f(t (String, String)) as String
   return t[0]
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static string f((string, string) t) {

    return t.Item1;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = @$""one"";
    var y = @$""two"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.f((x, y))));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "one\r\n");
    }

    [TestMethod]
    public void Pass_DeconstructIntoExistingVariables() {
        var code = @"#
main
    var x set to (3,""Apple"")
    var y set to 0
    var z set to """"
    set (y, z) to x
    print y
    print z
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
    var x = (3, @$""Apple"");
    var y = 0;
    var z = @$"""";
    (y, z) = x;
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
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_DeconstructIntoNewVariables() {
        var code = @"#
main
    var x set to (3,""Apple"")
    var (y, z) set to x
    print y
    print z
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
    var x = (3, @$""Apple"");
    var (y, z) = x;
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
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfSameType() {
        var code = @"#
main
    var x set to (3,""Apple"")
    set x to (4,""Pear"")
    print x
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
    var x = (3, @$""Apple"");
    x = (4, @$""Pear"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(4, Pear)\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_OutOfRangeError() {
        var code = @"#
main
    var x set to (3,""Apple"")
    print x[2]
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
    var x = (3, @$""Apple"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.Item3));
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
    public void Fail_AssignItemToWrongType() {
        var code = @"#
main
    var x set to (3,""Apple"")
    var y set to 4
    set y to x[1]
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
    public void Fail_ImmutableSoCannotAssignAnItem() {
        var code = @"#
main
    var x set to (3,""Apple"")
    set x[0] to 4
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify an element within a tuple");
    }

    [TestMethod]
    public void Fail_DeconstructIntoWrongType() {
        var code = @"#
main
    var x set to (3,""Apple"")
    var y set to 0
    var z set to """"
    set (z, y) to x
    print y
    print z
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
    public void Fail_DeconstructIntoMixed1() {
        var code = @"#
main
    var x set to (3,""Apple"")
    var z set to """"
    set (z, y) to x
    print y
    print z
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
    public void Fail_DeconstructIntoMixed2() {
        var code = @"#
main
    var x set to (3,""Apple"")
    var z set to """"
    var (z, y) set to x
    print y
    print z
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Duplicate id 'z' in scope 'main'");
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfWrongType() {
        var code = @"#
main
    var x set to (3,""Apple"")
    set x to (""4"",""Pear"")
    print x
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}