using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T48_InspectingTypes {
    #region Passes

    [TestMethod] // See commented out stubs in standard library
    public void Pass_typeMethod() {
        var code = @"#
main
    var x = 3
    var y = {'r','a','c'}
    var z = Foo()
    printLine(x.type())
    printLine(y.type())
    printLine(z.type())
    var s = ""Apple""
    s = x.Type()
    print(s)
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
        AssertObjectCodeExecutes(compileData, "Int\r\nList<Char>\r\nFoo\r\nInt\r\n");
    }

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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