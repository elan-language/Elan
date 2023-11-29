using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T31_LogicalOperators {
    [TestMethod]
    public void Pass_and() {
        var code = @"#
main
    var a set to false and false
    var b set to false and true
    var c set to true and false
    var d set to true and true
    print a
    print b
    print c
    print d
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
    var a = false && false;
    var b = false && true;
    var c = true && false;
    var d = true && true;
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\nfalse\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_or() {
        var code = @"#
main
    var a set to false or false
    var b set to false or true
    var c set to true or false
    var d set to true or true
    print a
    print b
    print c
    print d
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
    var a = false || false;
    var b = false || true;
    var c = true || false;
    var d = true || true;
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\ntrue\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_xor() {
        var code = @"#
main
    var a set to false xor false
    var b set to false xor true
    var c set to true xor false
    var d set to true xor true
    print a
    print b
    print c
    print d
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
    var a = false ^ false;
    var b = false ^ true;
    var c = true ^ false;
    var d = true ^ true;
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_not() {
        var code = @"#
main
    var a set to not false
    var b set to not true
    var c set to not not true
    var d set to not not false
    print a
    print b
    print c
    print d
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
    var a = !false;
    var b = !true;
    var c = !!true;
    var d = !!false;
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_Precedence() {
        var code = @"#
main
    var a set to not false and true
    var b set to not (false and true)
    print a
    print b
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
    var a = !false && true;
    var b = !(false && true);
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
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\n");
    }

    [TestMethod]
    public void Fail_UseNotWithTwoArgs() {
        var code = @"#
    main
      var a set to true not false
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}