using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T43_Inheritance
{
    #region Passes

    [TestMethod, Ignore]
    public void Pass_DefineAbstractClassAndInheritFromIt() {
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
  }
}";

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
 

    #endregion
}