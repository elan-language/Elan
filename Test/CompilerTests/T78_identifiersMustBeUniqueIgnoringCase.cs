using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T78_identifiersMustBeUniqueIgnoringCase
{
    #region Passes

    [TestMethod, Ignore]
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

    [TestMethod, Ignore]
    public void Pass_CanHaveIdentiferSameAsTypeExceptCase()
    {
        var code = @"#
main
    var string = ""Hello World!""
    printLine(string)
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
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }



    #endregion

    #region Fails

    [TestMethod, Ignore]
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


    [TestMethod, Ignore]
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

    [TestMethod, Ignore]
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


    [TestMethod, Ignore]
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

    [TestMethod]        
    public void Fail_SameVariableNameInScope()
    {
        var code = @"
main
    var id = 1
    var id = 1
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Duplicate id 'id' in scope Procedure: 'main'");
    }


    #endregion
}

