using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class TT138_ElanUnitTest {
    #region Passes

    [TestMethod]
    public void Pass_Assert() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
test squareHappyCase1
    assert square(3) is 9
end test

function square(x Int) as Int
    return x * x
end function

main
    var a set to square(3)
    print a
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int square(int x) {

    return x * x;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = Globals.square(3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}

[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
public class _Tests {
  [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
  public void squareHappyCase1() {
    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(9, Globals.square(3));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "9\r\n");
    }

    [TestMethod]
    public void Pass_ValidCode() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
test validCode
    var a set to 3
    var f set to new Foo(a)
    call f.square()
    assert f.p1 is 9
end test

class Foo 
    constructor(v Int)
        set p1 to v
    end constructor

    property p1 Int

    procedure square()
        set p1 to p1 * p1
    end procedure
end class

main
    print ""done""
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int v) {
      p1 = v;
    }
    public virtual int p1 { get; set; } = default;
    public virtual void square() {
      p1 = p1 * p1;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override void square() { }
      public string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""done""));
  }
}

[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
public class _Tests {
  [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
  public void validCode() {
    var a = 3;
    var f = new Foo(a);
    f.square();
    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(9, f.p1);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "done\r\n");
    }

    #endregion

    #region Fails

       [TestMethod]
    public void Fail_InvalidCode() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
test invalidCode
    var a set to 3
    var f set to new Foo(a)
    call f.square()
    print f.p1
    assert f.p1 is 9
end test

class Foo 
    constructor(v Int)
        set p1 to v
    end constructor

    property p1 Int

    procedure square()
        set p1 to p1 * p1
    end procedure
end class

main
    print ""done""
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    #endregion
}