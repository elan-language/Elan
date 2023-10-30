using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T49_EqualityTesting
{
    #region Passes

    [TestMethod]
    public void Pass_DifferentInstancesWithSameValuesAreEqual()
    {
        var code = @"#
main
    var x = Foo(7, ""Apple"")
    var z = Foo(7, ""Orange"")
    printLine(x is z)
end main

class Foo
    constructor(p1 Int, p2 String)
        self.p1 = p1
        self.p2 = p2
    end constructor

    property p1 Int
    property p2 String

    procedure setP1(v int)
        p1 = v
    end procedure

    function asString() -> String
      return ""{p1} {p2}""
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
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_ActuallyTheSameReference()
    {
        var code = @"#
main
    var x = Foo(7, ""Apple"")
    var y = x
    y.setP1(3)
    var z = Foo(8, ""Orange"")
    printLine(x is x)
    printLine(x is y)
    printLine(x is z)
end main

class Foo
    constructor(p1 Int, p2 String)
        self.p1 = p1
        self.p2 = p2
    end constructor

    property p1 Int
    property p2 String

    procedure setP1(v int)
        p1 = v
    end procedure

    function asString() -> String
      return ""{p1} {p2}""
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
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_StringEquality()
    {
        var code = @"#
main
    var x = ""Apple""
    var y = ""Apple""
    var z = ""apple""
    printLine(x is y)
    printLine(x is z)
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
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_ListEquality()
    {
        var code = @"#
main
    var x = {3,4,5}
    var y = {3,4,5}
    var z = {4,3,5}
    printLine(x is y)
    printLine(x is z)
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
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_ArraytEquality()
    {
        var code = @"#
main
    var x = {3,4,5}.asArray()
    var y = {3,4,5}.asArray()
    var z = {4,3,5}.asArray()
    var w = {3,4,5}
    printLine(x is y)
    printLine(x is z)
    printLine(x is w)
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
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_DictionaryEquality()
    {
        var code = @"#
main
    var x = {'a':3,'b':4,'c':5}
    var y = {'a':3,'b':4,'c':5}
    var z = {'b':4,'c':5,'a':3,}
    var w = {'a':3,'b':6,'c':5}
    var v = {""b"":4,""c"":5,""a"":3,}
    printLine(x is y)
    printLine(x is z)
    printLine(x is w)
    printLine(x is v)
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
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\nfalse\r\n");
    }

    #endregion

    #region Fails

    #endregion
}