using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T78_identifiersMustBeUniqueIgnoringCase
{
    #region Passes

    [TestMethod]
    public void Pass_CanUseCSharpKeywordWithDifferentCase()
    {
        var code = @"#
main
    var bReak = 1
    printLine(bReak)
end main
";

        var objectCode = @"";

        var parseTree = @"*";
        
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }



    #endregion

    #region Fails

    [TestMethod]
    public void Fail_DeclareSameVarNameWithDifferentCase()
    {
        var code = @"
main
    var fOO = 1
    var foo = 1
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "x");
    }


    [TestMethod]
    public void Fail_ElanKeywordWithChangedCase()
    {
        var code = @"
main
    var pRocedure = 1
end main
";
       var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "x");
    }

    [TestMethod]
    public void Fail_ElanKeywordTypeEvenWithChangedCase()
    {
        var code = @"
class Main 
    constructor()
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
        AssertDoesNotCompile(compileData, "x");
    }


    [TestMethod]
    public void Fail_CSharpKeywordWithCorrectCaseIfAlteredCaseAlreadyUsed()
    {
        var code = @"
main
    var bReak = 1
    var break = 1
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "x");
    }


    #endregion
}

