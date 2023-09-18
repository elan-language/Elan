using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_4_Constants
{
    #region Passes
    [TestMethod]
    public void Pass_Int()
    {
        var code = @"
main
  constant a = 3
  printLine(a)
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
    const int a = 3;
    printLine(a);
  }
}";

        var parseTree = @$"(file (main  main (statementBlock (constantDef  constant a = (literal (literalValue 3))) (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_Float()
    {
        var code = @"
main
  constant a = 3.1
  printLine(a)
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
    const double a = 3.1;
    printLine(a);
  }
}";

        var parseTree = @$"(file (main  main (statementBlock (constantDef  constant a = (literal (literalValue 3.1))) (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";


        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3.1\r\n");
    }

    [TestMethod]
    public void Pass_String()
    {
        var code = @"
main
  constant a = ""hell0""
  printLine(a)
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
    const string a = ""hell0"";
    printLine(a);
  }
}";

        var parseTree = @$"(file (main  main (statementBlock (constantDef  constant a = (literal (literalDataStructure ""hell0""))) (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";


        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "hell0\r\n");
    }

    [TestMethod]
    public void Pass_Char()
    {
        var code = @"
main
  constant a = 'a'
  printLine(a)
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
    const char a = 'a';
    printLine(a);
  }
}";

        var parseTree = @$"(file (main  main (statementBlock (constantDef  constant a = (literal (literalValue 'a'))) (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";


        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "a\r\n");
    }

    [TestMethod]
    public void Pass_Bool()
    {
        var code = @"
main
  constant a = true
  printLine(a)
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
    const bool a = true;
    printLine(a);
  }
}";

        var parseTree = @$"(file (main  main (statementBlock (constantDef  constant a = (literal (literalValue true))) (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";


        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "True\r\n");
    }

    [TestMethod]
    public void Pass_TopLevelConst()
    {
        var code = @"
constant a = 3
main
  printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {
  public const int a = 3;
}

public static class Program {
  private static void Main(string[] args) {
    printLine(a);
  }
}";

        var parseTree = $@"(file (constantDef  constant a = (literal (literalValue 3))) (main  main (statementBlock (callStatement  (expression (methodCall printLine ( (argumentList (expression (value a))) )))))  end main)  <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }


    [TestMethod]
    public void Fail_incorrectKeyword()
    {
        var code = @"
main
  const a = 3
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
    #endregion

    #region Fails

    [TestMethod]
    public void Fail_invalidLiteralString()
    {
        var code = @"
main
  constant a = 'hello'
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_invalidLiteralString2()
    {
        var code = @"
main
  constant a = hello
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_useBeforeDeclared()
    {
        var code = @"
main
  print(a)
  constant a = 3
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_reassignment()
    {
        var code = @"
main
  constant a = 3
  a = 4
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_expression()
    {
        var code = @"
main
  constant a = 3 + 4
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
        
    }
    #endregion
}

