using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T55_asString
{
    #region Passes

    [TestMethod]
    public void Pass_AsStringMayBeCalled()
    {
        var code = @"#
main
    var f = Foo()
    var s = f.asString()
    print(s)
end main
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String

    function asString() as String
         return p2
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
        AssertObjectCodeExecutes(compileData, "Apple");
    }

    [TestMethod]
    public void Pass_AsStringCalledWhenObjectPrinted()
    {
        var code = @"#
main
    var f = Foo()
    print(f)
end main
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String

    function asString() as String
         return p2
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
        AssertObjectCodeExecutes(compileData, "Apple");
    }

    [TestMethod]
    public void Pass_AsStringUsingDefaultHelper()
    {
        var code = @"#
main
    var f = Foo()
    print(f)
end main

class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String

    function asString() as String
         return self.typeAndProperties()
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
        AssertObjectCodeExecutes(compileData, "Foo {p1:5,p2:\"Apple\"}");
    }


    #endregion

    #region Fails

    [TestMethod]
    public void Fail_ClassHasNoAsString()
    {
        var code = @"#
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String
end class
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    #endregion
}