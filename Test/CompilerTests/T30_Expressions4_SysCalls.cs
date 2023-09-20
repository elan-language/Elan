using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T30_Expressions4_SystemCalls
{
    [TestMethod]
    public void Pass_LibraryConst() {
        var code = @"#
main
  var a = input(""Your name"")
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
    Console.WriteLine(pi);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3.141\r\n"); //Add full digits
    }

    [TestMethod]
    public void Pass_SingleFunctionCall()
    {
        var code = @"#
main
  var x = tan(pi/4)
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
    var x = sin(pi/180*30);
    Console.WriteLine(x);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0.5\r\n"); //Add full digits
    }

    [TestMethod]
    public void Pass_MoreComplexExpression()
    {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 + cos(x) ^ 2
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
     var x = 0.7;
    var y = pow(sin(x), 2) + pow(cos(x) ^ 2);
    Console.WriteLine(x);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0.5\r\n"); //Add full digits
    }


    [TestMethod]
    public void Fail_StandaloneFunctionCallNotValid()
    {
        var code = @"#
    main
      sin(1)
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData);
    }

}