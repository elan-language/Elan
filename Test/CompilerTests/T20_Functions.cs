using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T20_Functions {
    #region Passes

    [TestMethod]
    public void Pass_SimpleCase() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    print foo(3,4)
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int foo(int a, int b) {

    return a * b;
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.foo(3, 4)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_ReturnSimpleDefault() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    print foo(3,4)
end main

function foo(a Int, b Int) as Int
    return default
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int foo(int a, int b) {

    return default(int);
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.foo(3, 4)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0\r\n");
    }

    [TestMethod]
    public void Pass_ReturnClassDefault() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    print foo(3,4)
end main

function foo(a Int, b Int) as Foo
    return default
end function

class Foo
    constructor()
    end constructor
end class 
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static Foo foo(int a, int b) {

    return Foo.DefaultInstance;
  }
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {

    }


    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }


      public string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.foo(3, 4)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "default Foo\r\n");
    }

    [TestMethod]
    public void Pass_ReturnCollectionDefault() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    print foo(3,4)
end main

function foo(a Int, b Int) as Array<of Int>
    return default
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static StandardLibrary.ElanArray<int> foo(int a, int b) {

    return StandardLibrary.ElanArray<int>.DefaultInstance;
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.foo(3, 4)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "empty array\r\n");
    }

    [TestMethod]
    public void Pass_Recursive() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    print factorial(5)
end main

function factorial(a Int) as Int
    var result set to 0;
    if a > 2
        set result to a * factorial(a-1)
    else 
        set result to a
    end if
    return result
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int factorial(int a) {
    var result = 0;
    if (a > 2) {
      result = a * Globals.factorial(a - 1);
    }
    else {
      result = a;
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.factorial(5)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "120\r\n");
    }

    [TestMethod]
    public void Pass_GlobalFunctionOnClass() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var b set to new Bar()
    print b.foo()
end main

function foo(bar Bar) as String
    return bar.asString()
end function
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
  public static string foo(Bar bar) {

    return bar.asString();
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.foo(b)));
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

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_noEnd() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    call foo(3,4)
end main

function foo(a Int, b Int) as Int
    return a * b
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noReturnType() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int)
    return a * b
end function
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noAs() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) Int
    return a * b
end function
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noReturn() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) as Int
    var c set to a * b
end function
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_returnTypeIncompatible() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
   var a set to """"
   set a to foo(3,4)
end main

function foo(a Int, b Int) as Int
    var c set to a * b
    return c
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_noReturn2() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) as Int
    var c set to a * b
    return
end function
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_embeddedReturns() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) as Bool
    if 2 > 1
        return true
    else
        return false
    end if
end function
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_nonMatchingReturn2() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) as Int
    return a / b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_statementAfterReturn() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) as Int
    return a * b
    var c set to a + b
end function
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CanNotContainPrint() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) as Int
    print a
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot print in function");
    }

    [TestMethod]
    public void Fail_CanNotContainInput() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) as Int
    var x set to input
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot use 'input' within a function");
    }

    [TestMethod]
    public void Fail_CanNotContainSystemAccessors() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(a Int, b Int) as Int
    var r set to system.random()
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot access system within a function");
    }

    [TestMethod]
    public void Fail_CanNotContainProcedureCall() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var result set to foo(3,4)
    print result
end main

function foo(a Int, b Int) as Int
    call bar()
    return a * b
end function

procedure bar() 

end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot call a procedure within a function");
    }

    [TestMethod]
    public void Fail_CannotModifyParam() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var result set to foo(3,4)
    print result
end main

function foo(a Int, b Int) as Int
    set a to a + 1
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify param in function");
    }

    [TestMethod]
    public void Fail_CannotPassInArray() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
function foo(a Array<of Int>) as Int
    return a[0]
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot pass an Array into a function");
    }

    [TestMethod]
    public void Fail_CannotPassInArrayMultipleParameters() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
function foo(b Int, a Array<of Int>) as Int
    return a[0]
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot pass an Array into a function");
    }

    [TestMethod]
    public void Fail_TooManyParams() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var result set to foo(3,4,5)
    print result
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NotEnoughParams() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var result set to foo(3)
    print result
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_WrongParamType() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var result set to foo(3, ""b"")
    print result
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_CannotSpecifyParamByRef() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
end main

function foo(ref a Int, b Int) as Int
    return a * b
end function
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    #endregion
}