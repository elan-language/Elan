using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T51_ProcedureMethods
{
    #region Passes

    [TestMethod]
    public void Pass_HappyCase()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.p1)
    f.setP1(7)
    printLine(f.p1)
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    procedure setP1(value Int) 
        p1 = value
    end procedure

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
        AssertObjectCodeExecutes(compileData, "5\r\n7\r\n");
    }
    #endregion

    #region Fails
    #endregion
}