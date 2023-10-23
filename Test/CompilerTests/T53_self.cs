using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T53_self
{
    #region Passes

    [TestMethod]
    public void Pass_DisambiguateParamAndProperty()
    {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 as Int

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
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoSuchProperty()
    {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        self.p = p1
    end constructor

    property p1 as Int

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
    public void Fail_MissingSelfCausesCompileErrorDueToAssigningToParam()
    {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        p1 = p1
    end constructor

    property p1 as Int

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