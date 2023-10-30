using Compiler;
using StandardLibrary;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T54_With
{
    #region Passes

    [TestMethod]
    public void Pass_InstantiatingClassWithZeroParamConstructor()
    {
        var code = @"#
main
    var x = Foo() with {p1 = 3, p2 = ""Apple"" }
    printLine(x.p1)
    printLine(x.p2)
end main

class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 Int

    property p2 String

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
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_ConstructorWithParm()
    {
        var code = @"#
main
    var x = Foo(7) with {p1 = 3, p2 = ""Apple"" }
    printLine(x.p1)
    printLine(x.p2)
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor
    property p1 Int

    property p2 String

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
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_AppliedToInstanceButReturnedOneIsNewInstance()
    {
        var code = @"#
main
    var x = Foo()
    var y = x with {p1 = 3, p2 = ""Apple"" }
    printLine(y.p1)
    printLine(y.p2)
    printLine(x.p1)
end main

class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 Int

    property p2 String

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
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n5/r/n");
    }

    [TestMethod]
    public void Pass_WorksWithImmutableClass()
    {
        var code = @"#
main
    var x = Foo()
    var y = x with {p1 = 3, p2 = ""Apple"" }
    printLine(y.p1)
    printLine(y.p2)
    printLine(x.p1)
end main

immutable class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 Int

    property p2 String

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
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n5/r/n");
    }
    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NonMatchingProperty()
    {
        var code = @"#
main
    var x = Foo() with {p1 = 3, p3 = ""Apple"" }
    printLine(x.p1)
    printLine(x.p2)
end main

class Foo
    constructor()
        p1 = 5
    end constructor
    property p1 Int

    property p2 String

    function asString() -> String
         return """"
    end function

end class
";
        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }
    #endregion
}