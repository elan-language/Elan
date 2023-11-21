using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T65_ExpressionSyntaxFunctions {
    #region Fails

    [TestMethod]
    public void Fail_endFunction() {
        var code = @"
main
 print square(source[3])
end main

function square(x Int) as Int -> x * x
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion

    #region Passes

    [TestMethod]
    public void Pass_Simple() {
        var code = @"
main
 print square(source[3])
end main

function square(x Int) as Int -> x * x
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "49\r\n");
    }

    [TestMethod]
    public void Pass_NoParams() {
        var code = @"
main
 print phi()
end main

function phi() as Float -> (1 + sqrt(5))/2
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1.618...\r\n");
    }

    #endregion
}