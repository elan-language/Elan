﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T19_Procedures {
    [TestInitialize]
    public void TestInit() { }

    #region Passes

    [TestMethod]
    public void Pass_BasicOperationIncludingPrint() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
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
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    System.Console.WriteLine(StandardLibrary.Functions.asString(2));
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(1));
    Globals.foo();
    System.Console.WriteLine(StandardLibrary.Functions.asString(3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n2\r\n3\r\n");
    }

    [TestMethod]
    public void Pass_GlobalProcedureOnClass() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var b set to new Bar()
    call b.foo()
end main

procedure foo(bar Bar)
    print bar
end procedure
class Bar
    constructor()
    end constructor

    function asString() as String
        return ""bar""
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(Bar bar) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(bar));
  }
  public record class Bar {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {

    }

    public virtual string asString() {

      return @$""bar"";
    }
    private record class _DefaultBar : Bar {
      public _DefaultBar() { }


      public override string asString() { return ""default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var b = new Bar();
    Globals.foo(b);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "bar\r\n");
    }

    [TestMethod]
    public void Pass_SystemProcedure() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    call pause(1)
    print 1
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
    StandardLibrary.Procedures.pause(1);
    System.Console.WriteLine(StandardLibrary.Functions.asString(1));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_WithParamsPassingVariables() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 2
    var b set to ""hello""
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
    Globals.foo(a, b);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello\r\n");
    }

    [TestMethod]
    public void Pass_WithParamsPassingRefVariables() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 2
    var b set to ""hello""
    call foo(a, b)
end main

procedure foo(out a Int, out b String)
    print a
    print b
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a, ref string b) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 2;
    var b = @$""hello"";
    Globals.foo(ref a, ref b);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello\r\n");
    }

    [TestMethod]
    public void Pass_WithMixedRefParams() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 2
    var b set to true
    call foo(a, b)
end main

procedure foo(a Int, out b Bool)
    print a
    print b
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(int a, ref bool b) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 2;
    var b = true;
    Globals.foo(a, ref b);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\ntrue\r\n");
    }

    [TestMethod]
    public void Pass_CallingWithDotSyntax() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 2
    var b set to ""hello""
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
    Globals.foo(a, b);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello\r\n");
    }

    [TestMethod]
    public void Pass_WithParamsPassingLiteralsOrExpressions() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 1
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
    Globals.foo(a + 1, @$""hello"");
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello\r\n");
    }

    [TestMethod]
    public void Pass_RefParamsCanBeUpdated() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 1
    var b set to ""hello""
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
    Globals.foo(ref a, ref b);
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nhello!\r\n");
    }

    [TestMethod]
    public void Pass_NestedCalls() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
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
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    System.Console.WriteLine(StandardLibrary.Functions.asString(1));
    Globals.bar();
  }
  public static void bar() {
    System.Console.WriteLine(StandardLibrary.Functions.asString(2));
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.foo();
    System.Console.WriteLine(StandardLibrary.Functions.asString(3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n2\r\n3\r\n");
    }

    [TestMethod]
    public void Pass_Recursion() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    call foo(3)
end main

procedure foo(a Int)
    if a > 0
        print a
        call foo(a-1)
    end if
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(int a) {
    if (a > 0) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(a));
      Globals.foo(a - 1);
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.foo(3);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
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
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    call bar()
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_TypeSpecifiedBeforeParamName() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

procedure foo(Int a) 
    print a
end procedure
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoEnd() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    print 1
    call foo()
    print 3
end main

procedure foo()
    print 2
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CannotCallMain() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    print 1
    call foo()
    print 3
end main

procedure foo()
    call main()
end procedure
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_PassingUnnecessaryParameter() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
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

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_PassingTooFewParams() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 1
    call foo(a + 1)
end main

procedure foo (a Int, b String)
    print a
    print b
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_PassingWrongType() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    call foo(1,2)
end main

procedure foo (a Int, b String)
    print a
    print b
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InclusionOfOutInCall() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    call foo(out 1,2)
end main

procedure foo(a Int, b String)
    print a
    print b
end procedure
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InclusionOfRefInDefinition() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    call foo(byref 1,2)
end main

procedure foo(ref a Int, b String)
    print a
    print b
end procedure
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_UnterminatedRecursion() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
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
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(int a) {
    Globals.foo(a);
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.foo(3);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Stack overflow.");
    }

    [TestMethod]
    public void Fail_CannotCallPrintAsAProcedure() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  call print(""Hello World!"")
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NonRefParamsCannotBeUpdated() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 1
    var b set to ""hello""
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

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Parameter b may not be updated"); //Or similar message if shared with function rule
    }

    [TestMethod]
    public void Fail_RefKeywordMayNotBeAddedToArgument() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 1
    var b set to ""hello""
    call foo(ref a, b)
    print a
    print b
end main

procedure foo (ref a Int, b String)
    set a to a + 1
    set b to b + ""!""
end procedure
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_WithParamsPassingRefLiteral() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 2
    var b set to ""hello""
    call foo(a, ""hello"")
end main

procedure foo(out a Int, out b String)
    print a
    print b
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo(ref int a, ref string b) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
    System.Console.WriteLine(StandardLibrary.Functions.asString(b));
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 2;
    var b = @$""hello"";
    Globals.foo(ref a, ref @$""hello"");
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

    #endregion
}