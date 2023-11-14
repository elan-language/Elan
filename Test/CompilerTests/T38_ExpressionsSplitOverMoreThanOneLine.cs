using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T38_ExpressionsSplitOverMoreThanOneLine {
    #region Passes

    [TestMethod]
    public void Pass_1() {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 +
     cos(x) ^ 2
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0.7;
    var y = System.Math.Pow(StandardLibrary.Functions.sin(x), 2) + System.Math.Pow(StandardLibrary.Functions.cos(x), 2);
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
    public void Pass_3() {
        var code = @"#
main
  var x = 0.7
  var y = 
sin(x) ^ 2 + cos(x) ^ 2
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0.7;
    var y = System.Math.Pow(StandardLibrary.Functions.sin(x), 2) + System.Math.Pow(StandardLibrary.Functions.cos(x), 2);
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
    public void Pass_4() {
        var code = @"#
main
  var x = 0.7
  var y = sin(
        x) ^ 2  + cos(x) ^ 2
  print y
end main
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0.7;
    var y = System.Math.Pow(StandardLibrary.Functions.sin(x), 2) + System.Math.Pow(StandardLibrary.Functions.cos(x), 2);
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
    public void Pass_5() {
        var code = @"#
main
  var x = 0.7
  var y = 3 + 4 *
    1 + 2
  print y
end main
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 0.7;
    var y = 3 + 4 * 1 + 2;
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "9\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_1() {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 
     + cos(x) ^ 2
  print y
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_2() {
        var code = @"#
main
  var x = 0.7
  set y 
     to sin(x) ^ 2  + cos(x) ^ 2
  print y
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}