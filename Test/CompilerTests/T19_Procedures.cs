using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T19_Procedures
{
    #region Passes
    [TestMethod]
    public void Pass_CanContainSystemCalls()
    {
        var code = @"
main
    printLine(1)
    foo()
    printLine(3)
end main

procedure foo()
    printLine(2)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    printLine(1);
    foo();
    printLine(3);
  }

    private static void foo() {
        printLine(2);
    }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n2\r\n3\r\n");
    }

    [TestMethod]
    public void Pass_WithParamsPassingVariables()
    {
        var code = @"
main
    var a = 2
    var b = ""hello""
    foo(a, b)
end main

procedure foo(a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 2;
    var b = ""hello""
    foo(ref a, ref ""hello"");
  }

    private static void foo(ref int a, ref int b) {
        printLine(a);
        printLine(b);
    }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello\r\n");
    }

    [TestMethod]
    public void Pass_WithParamsPassingLiteralsOrExpressions()
    {
        var code = @"
main
    var a = 1
    foo(a + 1, ""hello"")
end main

procedure foo(a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = 1;
    foo(ref a + 1, ref ""hello"");
  }

    private static void foo(ref int a, ref int b) {
        printLine(a);
        printLine(b);
    }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello\r\n");
    }

    [TestMethod]
    public void Pass_paramsCanBeUpdated()
    {
        var code = @"
main
    var a = 1
    var b = ""hello""
    foo(a, b)
    printLine(a)
    printLine(b)
end main

procedure foo a Int, b String)
    a = a + 1
    b = b + ""!""
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
    private static void Main(string[] args) {
        var a = 1;
        var b = ""hello"";
        foo(a, b);
        printLine(a);
        printLine(b);
    }

    private static void foo(ref int a, ref int b) {
        a = a + 1;
        b = b + ""!"";
    }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello!\r\n");
    }

    [TestMethod]
    public void Pass_NestedCalls()
    {
        var code = @"
main
    foo()
    printLine(3)
end main

procedure foo()
    printLine(1)
    bar()
end procedure

procedure bar()
    printLine(2)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    foo();
    printLine(3);
  }

    private static void foo() {
        printLine(1);
        bar();
    }

    private static void bar() {
        printLine(2);
    }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n2\r\n3\r\n");
    }

    [TestMethod]
    public void Pass_Recursion()
    {
        var code = @"
main
    foo(3)
end main

procedure foo(a Int)
    if a > 0 then
        print(a)
    else
        foo(a-1)
    end if
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    foo(ref 3)
  }

    private static void foo(ref int a, ref int b) {
        if (a > 0) {
            print(a);
        }
        else {
            foo(ref a-1);
        }
    }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n2\r\n1\r\n");
    }

    #endregion

    #region Fails
    [TestMethod]
    public void Fail_NoEnd()
    {
        var code = @"
main
    printLine(1)
    foo()
    printLine(3)
end main

procedure foo()
    printLine(2)
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CannotCallMain()
    {
        var code = @"
main
    printLine(1)
    foo()
    printLine(3)
end main

procedure foo()
    main()
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_PassingUnnecessaryParameter()
    {
        var code = @"
main
    printLine(1)
    foo(3)
    printLine(3)
end main

procedure foo()
    printLine(2)
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_PassingTooFewParams()
    {
        var code = @"
main
    var a = 1
    foo(a + 1)
end main

procedure foo a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";


        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_PassingWrongType()
    {
        var code = @"
main
    foo(1,2)
end main

procedure foo a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";


        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InclusionOfRef()
    {
        var code = @"
main
    foo(ref 1,2)
end main

procedure foo a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InclusionOfByRef()
    {
        var code = @"
main
    foo(byref 1,2)
end main

procedure foo a Int, b String)
    printLine(a)
    printLine(b)
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_UnterminatedRecursion()
    {
        var code = @"
main
    foo(3)
end main

procedure foo(a Int)
    if a <10 then
        print(a)
    else
        foo(a-1)
    end if
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    foo(ref 3)
  }

    private static void foo(ref int a, ref int b) {
            foo(ref a-1);
    }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Stack Overflow Error");
    }
    #endregion
}