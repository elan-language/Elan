using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T29_Expressions3_FunctionCalls {
    #region Passes

    [TestMethod]
    public void Pass_LibraryConst() {
        var code = @"#
main
  print pi
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(pi));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3.141592653589793\r\n");
    }

    [TestMethod]
    public void Pass_SingleFunctionCall() {
        var code = @"#
main
  var x = sin(pi/180*30)
  print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = StandardLibrary.Functions.sin(Compiler.WrapperFunctions.FloatDiv(pi, 180) * 30);
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
        AssertObjectCodeExecutes(compileData, "0.49999999999999994\r\n");
    }

    [TestMethod]
    public void Pass_DotSyntax() {
        var code = @"#
main
  var x =  pi/180*30
  var y = x.sin()
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = Compiler.WrapperFunctions.FloatDiv(pi, 180) * 30;
    var y = StandardLibrary.Functions.sin(x);
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
        AssertObjectCodeExecutes(compileData, "0.49999999999999994\r\n");
    }

    [TestMethod]
    public void Pass_DotSyntaxFunctionEvaluationHasPrecedenceOverOperators() {
        var code = @"#
main
  var x =  pi/180*30
  var y = 2 + x.sin()
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = Compiler.WrapperFunctions.FloatDiv(pi, 180) * 30;
    var y = 2 + StandardLibrary.Functions.sin(x);
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
        AssertObjectCodeExecutes(compileData, "2.5\r\n"); //Add full digits
    }

    [TestMethod]
    public void Pass_MoreComplexExpression() {
        var code = @"#
main
  var x = 0.7
  var y = sin(x) ^ 2 + cos(x) ^ 2
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
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
    public void Pass_MultiParamCall() {
        var code = @"#
main
  var x = min(3.1, 3)
  print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = StandardLibrary.Functions.min(3.1, 3);
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
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_MultiParamCallUsingDotSyntax() {
        var code = @"#
main
  var x = 3.max(3.1)
  print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = StandardLibrary.Functions.max(3, 3.1);
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
        AssertObjectCodeExecutes(compileData, "3.1\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_IncorrectType() {
        var code = @"#
main
  var x = ""hello"".max(""world"")
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
    public void Fail_UnconsumedExpressionResult1() {
        var code = @"#
    main
      call sin(1)
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData, "Result generated by expression is not being used");
    }

    [TestMethod]
    public void Fail_UnconsumedExpressionResult2() {
        var code = @"#
    main
      1 + 2
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
       
    }

    [TestMethod]
    public void Fail_UnconsumedExpressionResult3() {
        var code = @"#
    main
      var a = 1
      call a.sin()
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertDoesNotCompile(compileData, "Result generated by expression is not being used");
    }

    #endregion
}