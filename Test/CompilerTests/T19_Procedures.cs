using Compiler;
using CSharpLanguageModel;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T19_Procedures {

    [TestInitialize]
    public void TestInit() {
        CodeHelpers.ResetUniqueId();
    }



    #region Passes

    [TestMethod]
    public void Pass_BasicOperationIncludingSystemCall() {
        var code = @"
main
    call printLine(1)
    call foo()
    call printLine(3)
end main

procedure foo()
    call printLine(2)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    printLine(2);
  }
}

public static class Program {
  private static void Main(string[] args) {
    printLine(1);
    foo();
    printLine(3);
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
    public void Pass_WithParamsPassingVariables() {
        var code = @"
main
    var a = 2
    var b = ""hello""
    call foo(a, b)
end main

procedure foo(a Int, b String)
    call printLine(a)
    call printLine(b)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a, ref string b) {
    printLine(a);
    printLine(b);
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 2;
    var b = @$""hello"";
    foo(ref a, ref b);
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
    public void Pass_WithParamsPassingLiteralsOrExpressions() {
        var code = @"
main
    var a = 1
    call foo(a + 1, ""hello"")
end main

procedure foo(a Int, b String)
    call printLine(a)
    call printLine(b)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a, ref string b) {
    printLine(a);
    printLine(b);
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 1;
    var _foo_1_0 = a + 1;
    var _foo_1_1 = @$""hello"";
    foo(ref _foo_1_0, ref _foo_1_1);
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
    public void Pass_paramsCanBeUpdated() {
        var code = @"
main
    var a = 1
    var b = ""hello""
    call foo(a, b)
    call printLine(a)
    call printLine(b)
end main

procedure foo (a Int, b String)
    set a to a + 1
    set b to b + ""!""
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a, ref string b) {
    a = a + 1;
    b = b + @$""!"";
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 1;
    var b = @$""hello"";
    foo(ref a, ref b);
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
        AssertObjectCodeExecutes(compileData, "2\r\nhello!\r\n");
    }

    [TestMethod]
    public void Pass_NestedCalls() {
        var code = @"
main
    call foo()
    call printLine(3)
end main

procedure foo()
    call printLine(1)
    call bar()
end procedure

procedure bar()
    call printLine(2)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    printLine(1);
    bar();
  }
  public static void bar() {
    printLine(2);
  }
}

public static class Program {
  private static void Main(string[] args) {
    foo();
    printLine(3);
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
    public void Pass_Recursion() {
        var code = @"
main
    call foo(3)
end main

procedure foo(a Int)
    if a > 0 then
        call printLine(a)
        call foo(a-1)
    end if
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a) {
    if (a > 0) {
      printLine(a);
      var _foo_1_0 = a - 1;
      foo(ref _foo_1_0);
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var _foo_2_0 = 3;
    foo(ref _foo_2_0);
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
    public void Fail_CallingUndeclaredProc() {
        var code = @"
main
    call bar()
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Calling unknown method");
    }

    [TestMethod]
    public void Fail_TypeSpecifiedBeforeParamName() {
        var code = @"
main
end main

procedure foo(Int a) 
    call printLine(a)
end procedure
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoEnd() {
        var code = @"
main
    call printLine(1)
    call foo()
    call printLine(3)
end main

procedure foo()
    call printLine(2)
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CannotCallMain() {
        var code = @"
main
    call printLine(1)
    call foo()
    call printLine(3)
end main

procedure foo()
    call main()
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_PassingUnnecessaryParameter() {
        var code = @"
main
    call printLine(1)
    call foo(3)
    call printLine(3)
end main

procedure foo()
    call printLine(2)
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_PassingTooFewParams() {
        var code = @"
main
    var a = 1
    call foo(a + 1)
end main

procedure foo (a Int, b String)
    call printLine(a)
    call printLine(b)
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_PassingWrongType() {
        var code = @"
main
    call foo(1,2)
end main

procedure foo (a Int, b String)
    call printLine(a)
    call printLine(b)
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InclusionOfRefInCall() {
        var code = @"
main
    call foo(ref 1,2)
end main

procedure foo(a Int, b String)
    call printLine(a)
    call printLine(b)
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InclusionOfRefInDefinition() {
        var code = @"
main
    call foo(byref 1,2)
end main

procedure foo(ref a Int, b String)
    call printLine(a)
    call printLine(b)
end procedure
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_UnterminatedRecursion() {
        var code = @"
main
    call foo(3)
end main

procedure foo(a Int)
    call foo(a)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a) {
    foo(ref a);
  }
}

public static class Program {
  private static void Main(string[] args) {
    var _foo_2_0 = 3;
    foo(ref _foo_2_0);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Stack overflow.");
    }

    #endregion
}