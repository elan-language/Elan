﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;


[TestClass]
public class T_9_Conditions
{
    #region Passes
    [TestMethod]
    public void Pass_lessThan()
    {
        var code = @"
main
  printLine(3 < 4)
  printLine(3 < 2)
  printLine(3 < 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 < 4);
    printLine(3 < 2);
    printLine(3 < 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <)) (expression (value (literal (literalValue 4)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <)) (expression (value (literal (literalValue 2)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_greaterThan()
    {
        var code = @"
main
  printLine(3 > 4)
  printLine(3 > 2)
  printLine(3 > 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 > 4);
    printLine(3 > 2);
    printLine(3 > 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp >)) (expression (value (literal (literalValue 4)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp >)) (expression (value (literal (literalValue 2)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp >)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_lessThanOrEqual()
    {
        var code = @"
main
  printLine(3 <= 4)
  printLine(3 <= 2)
  printLine(3 <= 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 <= 4);
    printLine(3 <= 2);
    printLine(3 <= 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <=)) (expression (value (literal (literalValue 4)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <=)) (expression (value (literal (literalValue 2)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <=)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_greaterThanOrEqual()
    {
        var code = @"
main
  printLine(3 >= 4)
  printLine(3 >= 2)
  printLine(3 >= 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 >= 4);
    printLine(3 >= 2);
    printLine(3 >= 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp >=)) (expression (value (literal (literalValue 4)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp >=)) (expression (value (literal (literalValue 2)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp >=)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_greaterOrLessThan()
    {
        var code = @"
main
  printLine(3 <> 4)
  printLine(3 <> 2)
  printLine(3 <> 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 != 4);
    printLine(3 != 2);
    printLine(3 != 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <>)) (expression (value (literal (literalValue 4)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <>)) (expression (value (literal (literalValue 2)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <>)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_isNot()
    {
        var code = @"
main
  printLine(3 is not 4)
  printLine(3 is not 2)
  printLine(3 is not 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 != 4);
    printLine(3 != 2);
    printLine(3 != 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp is not)) (expression (value (literal (literalValue 4)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp is not)) (expression (value (literal (literalValue 2)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp is not)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_equalTo()
    {
        var code = @"
main
  printLine(3 == 4)
  printLine(3 == 2)
  printLine(3 == 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 == 4);
    printLine(3 == 2);
    printLine(3 == 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp ==)) (expression (value (literal (literalValue 4)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp ==)) (expression (value (literal (literalValue 2)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp ==)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_is()
    {
        var code = @"
main
  printLine(3 is 4)
  printLine(3 is 2)
  printLine(3 is 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 == 4);
    printLine(3 == 2);
    printLine(3 == 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp is)) (expression (value (literal (literalValue 4)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp is)) (expression (value (literal (literalValue 2)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp is)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_canCompareCoerdableTypes()
    {
        var code = @"
main
  printLine(3 < 3.1)
  printLine(3 == 3.0)
  printLine(3.1 < 3)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 < 3.1);
    printLine(3 == 3.0);
    printLine(3.1 < 3);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp <)) (expression (value (literal (literalValue 3.1)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp ==)) (expression (value (literal (literalValue 3.0)))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3.1)))) (binaryOp (conditionalOp <)) (expression (value (literal (literalValue 3)))))) ))))) end main) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
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
    public void Fail_not_is()
    {
        var code = @"
main
  printLine(3 not is 3)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_not()
    {
        var code = @"
main
  printLine(3 not 3)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_notEqual()
    {
        var code = @"
main
  printLine(3 != 3)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_EqualToOrLessThan()
    {
        var code = @"
main
  printLine(3 =< 3)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_Greater_Or_Equal()
    {
        var code = @"
main
  printLine(3 > or = 3)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_SingleEquals()
    {
        var code = @"
main
  printLine(3 = 4)
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }


    [TestMethod]
    public void Fail_compareDifferentTypes()
    {
        var code = @"
main
  printLine(3 is ""3"")
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(3 == ""3"");
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value (literal (literalValue 3)))) (binaryOp (conditionalOp is)) (expression (value (literal (literalDataStructure ""3"")))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeDoesNotCompile(compileData);
    }
    #endregion

}