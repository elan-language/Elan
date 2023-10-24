using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T52_FunctionMethods
{
    #region Passes

    [TestMethod]
    public void Pass_HappyCase()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.times(2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) as Int
        return p1 * value
    end function

    function asString() as String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public class Foo {
    public Foo() {
      p1 = 5;
    }
    public int p1 { get; set; }
    public int times(int value) {

      return p1 * value;
    }
    public string asString() {

      return @$"""";
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f.times(2));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod. Ignore]
    public void Pass_FunctionMethodMayCallOtherFunctionMethod()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.times(2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) as Int
        return p1PlusOne() * value
    end function

    function p1PlusOne() as Int
        return p1 +1 
    end function

    function asString() as String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public class Foo {
    public Foo() {
      p1 = 5;
    }
    public int p1 { get; set; }
    public int times(int value) {

      return p1PlusOne() * value;
    }
    public int p1PlusOne() {

      return p1 + 1;
    }
    public string asString() {

      return @$"""";
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f.times(2));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }


    #endregion

    #region Fails

    [TestMethod]
    public void Fail_FunctionCannotBeCalledDirectly()
    {
        var code = @"#
main
    var f = Foo()
    printLine(times(f,2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) as Int
        return p1 * value
    end function

    function asString() as String
         return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_FunctionMethodCannotMutateProperty()
    {
        var code = @"#
class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) as Int
        p1 = p1 * value
        return p1
    end function

    function asString() as String
         return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_FunctionMethodCannotCallProcedureMethod()
    {
        var code = @"#
class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) as Int
        setP1(p1 * value)
        return p1
    end function

    procedure setP1(value Int) 
        p1 = value
    end procedure

    function asString() as String
         return """"
    end function

end class
";

       var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }
    #endregion
}