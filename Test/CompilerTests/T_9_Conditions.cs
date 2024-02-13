using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_9_Conditions {
    #region Passes

    [TestMethod]
    public void Pass_lessThan() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 < 4
  print 3 < 2
  print 3 < 3
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 < 4));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 < 2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 < 3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_greaterThan() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 > 4
  print 3 > 2
  print 3 > 3
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 > 4));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 > 2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 > 3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_lessThanOrEqual() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 <= 4
  print 3 <= 2
  print 3 <= 3
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 <= 4));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 <= 2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 <= 3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_greaterThanOrEqual() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 >= 4
  print 3 >= 2
  print 3 >= 3
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 >= 4));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 >= 2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 >= 3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_isNot() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 is not 4
  print 3 is not 2
  print 3 is not 3
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 != 4));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 != 2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 != 3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_is() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 is 4
  print 3 is 2
  print 3 is 3
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 == 4));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 == 2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 == 3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_canCompareCoerdableTypes() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 < 3.1
  print 3 is 3.0
  print 3.1 < 3
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 < 3.1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 == 3.0));
    System.Console.WriteLine(StandardLibrary.Functions.asString(3.1 < 3));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_not_is() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 not is 3
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_not() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 not 3
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_notEqual() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 != 3
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_EqualToOrLessThan() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 =< 3
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_Greater_Or_Equal() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 > or = 3
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_SingleEquals() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 = 4
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_compareDifferentTypes() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 is ""3""
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(3 == @$""3""));
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
    public void Fail_greaterOrLessThan() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 <> 4
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_doubleEquals() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print 3 == 4
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    #endregion
}