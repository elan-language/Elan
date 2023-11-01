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
        var code = @"#
class Foo
    constructor(p_1 Int)
        p_1 = p1
    end constructor

    property p1 Int

    function asString() -> String
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
    public void Fail_MutatingArrayParam() {
        var code = @"#
class Foo
    constructor(a Array<Int>)
        a[0] = 4
    end constructor

    function asString() -> String
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