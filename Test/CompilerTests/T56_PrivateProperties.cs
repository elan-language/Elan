using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T56_PrivateProperties
{
    #region Passes

    [TestMethod]
    public void Pass_PrivatePropertyCanBeDeclared()
    {
        var code = @"#
class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String

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
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_PrivatePropertyCannotBeAccessed()
    {
        var code = @"#
main
    var f = Foo()
    var s = f.p2
end main

class Foo
    constructor()
        p1 = 5
        p2 = ""Apple""
    end constructor

    property p1 as Int

    private property p2 as String

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
        AssertDoesNotCompile(compileData);
    }

    #endregion
}