using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T57_ConstructorParmsNotMutable {
    #region Passes

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_reassigningIntParam() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
class Foo
    constructor(p_1 Int)
        set p_1 to p1
    end constructor

    property p1 Int

    function asString() as String
        return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify param in constructor");
    }

    [TestMethod]
    public void Fail_MutatingArrayParam() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
class Foo
    constructor(a Array<of Int>)
        set a[0] to 4
    end constructor

    function asString() as String
        return """"
    end function
end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify param in constructor");
    }

    #endregion
}