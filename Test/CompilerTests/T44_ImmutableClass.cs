using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T44_ImmutableClass
{
    #region Passes

    [TestMethod, Ignore]
    public void Pass_BasicImmutableClass()
    {
        var code = @"#
main
    var f = Foo(3)
    printLine(f.p1)
    printLine(f.square())
end main

immutable class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 Int

    function square() -> Int
        return p1 * p1
    end function
    
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
        AssertObjectCodeExecutes(compileData, "3\r\n9\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_AbstractImmutableClass()
    {
        var code = @"#
main
    var f = Foo(3)
    printLine(f.p1)
    printLine(f.square())
end main

abstract immutable class Bar
    property p1 Int

    function square() -> Int
end class

immutable class Foo inherits bar
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    function square() -> Int
        return p1 * p1
    end function
    
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
        AssertObjectCodeExecutes(compileData, "3\r\n9\r\n");
    }

    #endregion

    #region Fails
    // can't have a procedure method
    // inheriting from an abstract immutable must specify immutable
    // immutable and abstract being the wrong way around

    [TestMethod, Ignore]
    public void Fail_ProcedureMethod()
    {
        var code = @"#
immutable class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure
    
    function asString() -> String
        return """"
    end function
end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }


    [TestMethod, Ignore]
    public void Pass_ProcedureMethodOnAbstractImmutableClass()
    {
        var code = @"#
abstract immutable class Bar
    property p1 Int

    procedure setP1(v Int)
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Pass_AbstractAndImmutableReversed()
    {
        var code = @"#
immutable abstract class Bar
    property p1 Int

    procedure setP1(v Int)
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
    #endregion
}