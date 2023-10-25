using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T43_Inheritance
{
    #region Passes

    [TestMethod]
    public void Pass_DefineAbstractClassAndInheritFromIt() {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
    printLine(x.p1)
    printLine(x.p2)
    printLine(x.product())
    x.setP1(4)
    printLine(x.product())
end main

abstract class Foo
    property p1 as Int
    property p2 as Int

    procedure setP1(v Int)

    function product() as Int
end class

class Bar inherits Foo
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 as Int
    property p2 as Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() as Int
        return p1 * p2
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
  public interface Foo {
    public int p1 { get; set; }
    public int p2 { get; set; }
    public int product();
    public void setP1(ref int v);
  }
  public class Bar : Foo {
    public Bar() {
      p1 = 3;
      p2 = 4;
    }
    public int p1 { get; set; }
    public int p2 { get; set; }
    public int product() {

      return p1 * p2;
    }
    public string asString() {

      return @$"""";
    }
    public void setP1(ref int p1) {
      this.p1 = p1;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Bar();
    var l = new StandardLibrary.ElanList<Foo>() + x;
    printLine(x.p1);
    printLine(x.p2);
    printLine(x.product());
    var _setP1_0 = 4;
    x.setP1(ref _setP1_0);
    printLine(x.product());
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n12\r\n16\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    [TestMethod, Ignore]
    public void Pass_InheritFromMoreThanOneAbstractClass()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

abstract class Foo
    property p1 as Int
    property p2 as Int
end class

abstract class Yon
    procedure setP1(v Int)
    function product() as Int
end class

class Bar inherits Foo, Yon
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 as Int
    property p2 as Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() as Int
        return p1 * p2
    end function

    function asString() as String 
        return """"
    end function
end class
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n0\r\n\r\n3\r\n15\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    [TestMethod, Ignore]
    public void Pass_SuperclassesCanDefineSameMember()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

abstract class Foo
    property p1 as Int
    property p2 as Int
end class

abstract class Yon
    property p1 as Int
    procedure setP1(v Int)
    function product() as Int
end class

class Bar inherits Foo, Yon
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 as Int
    property p2 as Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() as Int
        return p1 * p2
    end function

    function asString() as String 
        return """"
    end function
end class
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n0\r\n\r\n3\r\n15\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    #endregion

    #region Fails
    [TestMethod, Ignore]
    public void Fail_CannotInheritFromConcreteClass()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

class Foo
    property p1 as Int
    property p2 as Int
end class

class Bar inherits Foo
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 as Int
    property p2 as Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() as Int
        return p1 * p2
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

    [TestMethod, Ignore]
    public void Fail_MustImplementAllInheritedMethods()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

abstract class Foo
    property p1 as Int
    property p2 as Int

    procedure setP1(v Int)

    function product() as Int
end class

class Bar inherits Foo
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 as Int
    property p2 as Int

    procedure setP1(p1 Int)
        self.p1 = p1
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

    [TestMethod, Ignore]
    public void Fail_ImplementedMethodMustHaveSameSignature()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

abstract class Foo
    property p1 as Int
    property p2 as Int

    procedure setP1(v Int)

    function product() as Int
end class

class Bar inherits Foo
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 as Int
    property p2 as Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() as Float
        return p1 * p2
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

    [TestMethod, Ignore]
    public void Pass_AbstractClassDefinesMethodBody()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

abstract class Foo
    property p1 as Int
    property p2 as Int

    procedure setP1(v Int)
        self.p1 = p1
    end procedure

    function product() as Int
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}