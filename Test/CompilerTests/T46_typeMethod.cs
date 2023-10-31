using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T46_typeMethod
{
    #region Passes

    [TestMethod]
    public void Pass_Template() {
        var code = @"
main
    printLine(1.type());
    printLine(1.1.type());
    printLine('a'.type());
    printLine(false.type());
    printLine(""a"".type());
    printLine({1,2,3}.type());
    printLine({'a': 3}.type());
    printLine({1,2,3}.asArray().type());
    printLine(Foo(3).type());
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1 * 2
    end constructor

    property p1 Int

    function asString() -> String
        return ""{p1}""
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
        AssertObjectCodeExecutes(compileData, "Int\r\nFloat\r\nChar\r\nBool\r\nString\r\nList<Int>\r\nDictionary<Char,Int>\r\nArray<Int>\r\nFoo\r\n");
    }

    [TestMethod]
    public void Pass_TypeTesting()
    {
        var code = @"
main
    printLine(1.type() is 2.type());
    printLine(1.0.type() is 2.type());
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1 * 2
    end constructor

    property p1 Int

    function asString() -> String
        return ""{p1}""
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
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    #endregion

    #region Fails

    #endregion
}