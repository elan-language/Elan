using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T28_Expressions2_Brackets
{
    [TestMethod]
    public void Pass_BracketsChangeOperatorEvaluation() {
        var code = @"#
main
  var x = 2 + 3 * 5 + 1
  var y = (2 + 3) * 5 + 1
  var z = (2 + 3) * (5 + 1)
  printLine(x)
  printLine(y)
  printLine(z)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
    var x = 2 + 3 * 5 + 1
    var y = (2 + 3) * 5 + 1
    var z = (2 + 3) * (5 + 1)
    printLine(x);
    printLine(y);
    printLine(z);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "18\r\n26\r\n30\r\n");
    }

    [TestMethod]
    public void Pass_RedundantBracketsIgnored()
    {
        var code = @"#
main
  var x = 2 + (3 * 5) + 1
  var y = ((2 + 3)) * 5 + (1)
  var z = ((2 + 3) * (5 + 1))
  printLine(x)
  printLine(y)
  printLine(z)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
    var x = 2 + 3 * 5 + 1
    var y = (2 + 3) * 5 + 1
    var z = (2 + 3) * (5 + 1)
    printLine(x);
    printLine(y);
    printLine(z);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "18\r\n26\r\n30\r\n");
    }

    [TestMethod]
    public void Pass_PowerHasPrecedenceOverMultiply()
    {
        var code = @"#
main
  var x = 2 + 3 ^ 2
  var y = (2 + 3) ^ 2
  printLine(x)
  printLine(y)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
    var x = 2 + 3 ^ 2
    var y = (2 + 3) ^ 2
    printLine(x);
    printLine(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "11\r\n25\r\n");
    }

    [TestMethod]
    public void Pass_MinusAsAUnaryOperator()
    {
        var code = @"#
main
  var x = 5 * -3
  printLine(x)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var x = - 4.7
    var y = 5 * -3;
    printLine(x);
    printLine(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "-4.7\r\n-15\r\n");
    }

    [TestMethod]
    public void Fail_PlusIsNotUnary()
    {
        var code = @"#
    main
      var a = 3 * + 4
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_MultiplyAfterMinus()
    {
        var code = @"#
    main
      var a = 3 - * 4
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

}