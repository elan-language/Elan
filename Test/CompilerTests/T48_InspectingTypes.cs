using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T48_InspectingTypes {
    #region Passes

    [TestMethod, Ignore] // See commented out stubs in standard library
    public void Pass_typeMethod() {
        var code = @"#
main
    var a = 3
    var b = 3.0
    var c = 'z'
    var d = ""z""
    var e = true
    var f = {'r','a','c'}
    var g = {'a':1, 'b':3, 'z':10}
    var h = {'r','a','c'}.asArray()
    var i = Foo()

    printLine(a.type())
    printLine(b.type())
    printLine(c.type())
    printLine(d.type())
    printLine(e.type())
    printLine(f.type())
    printLine(g.type())
    printLine(h.type())
    printLine(i.type())


end main

class Foo 
    constructor()
    end constructor

    function asString() -> String
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
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {

    }

    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }


      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = 3;
    var b = 3.0;
    var c = 'z';
    var d = @$""z"";
    var e = true;
    var f = new StandardLibrary.ElanList<char>('r', 'a', 'c');
    var g = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('z', 10));
    var h = asArray(new StandardLibrary.ElanList<char>('r', 'a', 'c'));
    var i = new Foo();
    printLine(type(a));
    printLine(type(b));
    printLine(type(c));
    printLine(type(d));
    printLine(type(e));
    printLine(type(f));
    printLine(type(g));
    printLine(type(h));
    printLine(type(i));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Int\r\nFloat\r\nChar\r\nString\r\nBool\r\nList<Char>\r\nDictionary<Char, Int>\r\nArray<Char>\r\nFoo\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_testTypes() {
        var code = @"#
main
    var x = 3
    var y = {'r','a','c'}
    var z = Foo()
    printLine(x.isType(Float))
    printLine(y.isType(List<Int>))
    printLine(z.isType(Foo))
    printLine(x.type().isType(String))
end main

class Foo 
    constructor()
    end constructor

    function asString() -> String
        return """"
    end function

end class
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\ntrue\r\ntrue\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_testSubType() {
        var code = @"#
main
    var x = 3
    var y = {'r','a','c'}
    var z = Foo()
    printLine(x.isSubTypeOf(Int))
    printLine(z.isSubTypeOf(Bar))
end main

abstract class Bar
end class

class Foo inherits Bar
    constructor()
    end constructor

    function asString() -> String
        return """"
    end function

end class
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "false\r\ntrue\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_Tuple() {
        var code = @"#
main
    var x = (3, ""Apple"")
    printLine(x.type())
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(Int, String)\r\n");
    }

    #endregion

    #region Fails

    #endregion
}