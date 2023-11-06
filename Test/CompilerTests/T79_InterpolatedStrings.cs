﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T79_InterpolatedStrings
{
    #region Passes

    [TestMethod]
    public void Pass_CanUseVariables() {
        var code = @"
main
    var a = 1
    var b = ""Apple""
    var c = {1,2,3}
    printLine(""{a} {b} {c}"")
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
        AssertObjectCodeExecutes(compileData, "1 Apple List {1,2,3}\r\n");
    }

    [TestMethod]
    public void Pass_UseCSharpKeywordAsVariable()
    {
        var code = @"
main
    var new = 1
    printLine(""{new}"")
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
    public void Fail_useExpression()
    {
        var code = @"
main
    var a = 1
    printLine(""{a + 1}"")
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }


    #endregion
}