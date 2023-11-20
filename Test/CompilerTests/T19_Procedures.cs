﻿using Compiler;
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
    public void Pass_BasicOperationIncludingPrint() {
        var code = @"
main
    print 1
    call foo()
    print 3
end main

procedure foo()
    print 2
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    System.Console.WriteLine(StandardLibrary.Functions.asString(2));
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(1));
    foo();
    System.Console.WriteLine(StandardLibrary.Functions.asString(3));
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
    print a
    print b
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(int a, string b) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 2;
    var b = @$""hello"";
    foo(a, b);
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
    public void Pass_CallingWithDotSyntax()
    {
        var code = @"
main
    var a = 2
    var b = ""hello""
    call a.foo(b)
end main

procedure foo(a Int, b String)
    print a
    print b
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(int a, string b) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 2;
    var b = @$""hello"";
    foo(a, b);
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
    print a
    print b
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(int a, string b) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 1;
    foo(a + 1, @$""hello"");
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
    public void Pass_RefParamsCanBeUpdated() {
        var code = @"
main
    var a = 1
    var b = ""hello""
    call foo(a, b)
    print a
    print b
end main

procedure foo (out a Int, out b String)
    set a to a + 1
    set b to b + ""!""
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
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
    print 3
end main

procedure foo()
    print 1
    call bar()
end procedure

procedure bar()
    print 2
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    System.Console.WriteLine(StandardLibrary.Functions.asString(1));
    bar();
  }
  public static void bar() {
    System.Console.WriteLine(StandardLibrary.Functions.asString(2));
  }
}

public static class Program {
  private static void Main(string[] args) {
    foo();
    System.Console.WriteLine(StandardLibrary.Functions.asString(3));
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
        print a
        call foo(a-1)
    end if
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(int a) {
    if (a > 0) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(a));
      foo(a - 1);
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    foo(3);
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
    print a
end procedure
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoEnd() {
        var code = @"
main
    print 1
    call foo()
    print 3
end main

procedure foo()
    print 2
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CannotCallMain() {
        var code = @"
main
    print 1
    call foo()
    print 3
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
    print 1
    call foo(3)
    print 3
end main

procedure foo()
    print 2
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
    print a
    print b
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
    print a
    print b
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
    public void Fail_InclusionOfOutInCall() {
        var code = @"
main
    call foo(out 1,2)
end main

procedure foo(a Int, b String)
    print a
    print b
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
    print a
    print b
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
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(int a) {
    foo(a);
  }
}

public static class Program {
  private static void Main(string[] args) {
    foo(3);
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

    [TestMethod]
    public void Fail_CannotCallPrintAsAProcedure()
    {
        var code = @"#
main
  call print(""Hello World!"")
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NonRefParamsCannotBeUpdated()
    {
        var code = @"
main
    var a = 1
    var b = ""hello""
    call foo(a, b)
    print a
    print b
end main

procedure foo (out a Int, b String)
    set a to a + 1
    set b to b + ""!""
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Parameter b may not be updated"); //Or similar message if shared with function rule
    }

    [TestMethod]
    public void Fail_RefKeywordMayNotBeAddedToArgument()
    {
        var code = @"
main
    var a = 1
    var b = ""hello""
    call foo(ref a, b)
    print a
    print b
end main

procedure foo (ref a Int, b String)
    set a to a + 1
    set b to b + ""!""
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}