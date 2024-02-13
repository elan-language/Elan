using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T62_Tuples {
    #region Passes

    [TestMethod]
    public void Pass_CreatingTuplesAndReadingContents() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to (3,""Apple"")
    print x
    print x.first()
    print x.second()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.first(x)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.second(x)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(3, Apple)\r\n3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_FunctionReturnsTuple() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to f()
    print x
    print x.first()
    print x.second()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.first(x)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.second(x)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(1, 2)\r\n1\r\n2\r\n");
    }

    [TestMethod]
    public void Pass_IndexFunctionReturnsTuple() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    print f().first()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.first(Globals.f())));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_IndexGenericFunctionReturnsTuple() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    print a.reduce((1,1), lambda i, j -> j).first()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.first(StandardLibrary.Functions.reduce(a, (1, 1), (i, j) => j))));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_FunctionTupleParameter() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to ""one""
    var y set to ""two""
    print f((x,y))
end main
function f(t (String, String)) as String
   return t.first()
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static string f((string, string) t) {

    return StandardLibrary.Functions.first(t);
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

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "one\r\n");
    }

    [TestMethod]
    public void Pass_DeconstructIntoExistingVariables() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
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

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_DeconstructIntoNewVariables() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
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

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfSameType() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
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

        var compileData = Pipeline.Compile(CompileData(code));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to (3,""Apple"")
    print x.third()
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.third()));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AssignItemToWrongType() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to (3,""Apple"")
    var y set to 4
    set y to x.second()
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
    public void Fail_ImmutableSoCannotAssignAnItem() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to (3,""Apple"")
    set x.first() to 4
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_DeconstructIntoWrongType() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
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

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_DeconstructIntoMixed1() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to (3,""Apple"")
    var z set to """"
    set (z, y) to x
    print y
    print z
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
    public void Fail_DeconstructIntoMixed2() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to (3,""Apple"")
    var z set to """"
    var (z, y) set to x
    print y
    print z
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Duplicate id 'z' in scope 'main'");
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfWrongType() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to (3,""Apple"")
    set x to (""4"",""Pear"")
    print x
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}