using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_5_Variables {
    #region Passes

    [TestMethod]
    public void Pass_Int() {
        var code = @"#
main
  var a set to 3
  print a
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
    var a = 3;
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
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_Int_Expression() {
        var code = @"#
main
  var a set to 3 + 4
  print a
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
    var a = 3 + 4;
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
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    [TestMethod]
    public void Pass_Reassign() {
        var code = @"#
main
  var a set to 3
  set a to 4
  print a
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
    var a = 3;
    a = 4;
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
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    [TestMethod]
    public void Pass_CoerceFloatToIntVar() {
        var code = @"#
main
  var a set to 3.1
  set a to 4
  print a
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
    var a = 3.1;
    a = 4;
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
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    [TestMethod]
    public void Pass_LocalVarHidesGlobalConstant() {
        var code = @"#
constant a set to 3
main
  var a set to 4
  print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public const int a = 3;
}

public static class Program {
  private static void Main(string[] args) {
    var a = 4;
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
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

  

    [TestMethod]
    public void Pass_OperatorCoverage() {
        var code = @"#
main
  var a set to 3 - 4
  var b set to 3 < 4
  var c set to 3 <= 4
  var d set to 3 > 4
  var e set to 3 >= 4
  var f set to 3 is 4
  var g set to 3 is not 4
  var h set to not false
  print a
  print b
  print c
  print d
  print e
  print f
  print g
  print h
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
    var a = 3 - 4;
    var b = 3 < 4;
    var c = 3 <= 4;
    var d = 3 > 4;
    var e = 3 >= 4;
    var f = 3 == 4;
    var g = 3 != 4;
    var h = !false;
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
    System.Console.WriteLine(StandardLibrary.Functions.asString(c));
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
    System.Console.WriteLine(StandardLibrary.Functions.asString(e));
    System.Console.WriteLine(StandardLibrary.Functions.asString(f));
    System.Console.WriteLine(StandardLibrary.Functions.asString(g));
    System.Console.WriteLine(StandardLibrary.Functions.asString(h));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "-1\r\ntrue\r\ntrue\r\nfalse\r\nfalse\r\nfalse\r\ntrue\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_Enum() {
        var code = @"#
main
  var a set to Fruit.apple
  print a
end main
enum Fruit
    apple, orange, pear
end enum
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public enum Fruit {
    apple,
    orange,
    pear,
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = Fruit.apple;
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
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }
   

    #endregion

    #region Fails

    [TestMethod]
    public void fail_wrongKeyword() {
        var code = @"#
main
  variable a = 3
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_duplicateVar() {
        var code = @"#
main
  var a set to 3
  var a set to 4
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData, "Duplicate id 'a' in scope 'main'");
    }

    [TestMethod]
    public void fail_GlobalVariable() {
        var code = @"#
var a = 4
main
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_assignIncompatibleType() {
        var code = @"#
main
  var a set to 4
  set a to 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void fail_notInitialised() {
        var code = @"#
main
  var a
  set a to 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_invalidVariableName1() {
        var code = @"#
main
  var A = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_invalidVariableName2() {
        var code = @"#
main
  var a@b = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void fail_useOfKeywordAsName() {
        var code = @"#
main
  var if = 4.1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}