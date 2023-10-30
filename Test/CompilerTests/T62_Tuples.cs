using Compiler;
using StandardLibrary;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T62_Tuples
{
    #region Passes

    [TestMethod]
    public void Pass_CreatingTuplesAndReadingContents()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    printLine(x)
    printLine(x[0])
    printLine(x[1])
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(3,Apple)\r\n3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_DeconstructIntoExistingVariables()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 0
    var z = ""
    (y, z) = x
    printLine(y)
    printLine(z)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_DeconstructIntoNewVariables()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    (var y, var z) = x
    printLine(y)
    printLine(z)
end main
";
        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\nApple\r\n");
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfSameType()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    x = (4,""Pear"")
    printLine(x)
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(4,Pair)\r\n");
    }

    #endregion

    #region Fails
    [TestMethod]
    public void Fail_OutOfRangeError()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    printLine(x[2])
end main
";

        var objectCode = @"";

        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Some error");
    }

    [TestMethod]
    public void Fail_AssignItemToWrongType()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 4
    y = x[1]
end main
";
        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_ImmutableSoCannotAssignAnItem()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    x[0] = 4
end main
";
        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }


    [TestMethod]
    public void Fail_DeconstructIntoWrongType()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    var y = 0
    var z = ""
    (z, y) = x
    printLine(y)
    printLine(z)
end main
";
        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Pass_AssignANewTupleOfWrongType()
    {
        var code = @"#
main
    var x = (3,""Apple"")
    x = ('4',""Pear"")
    printLine(x)
end main
";
        var parseTree = @"";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }
    #endregion
}