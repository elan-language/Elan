using Compiler;
using Test.CompilerTests;

namespace Test.ExampleProjects;

using static Helpers;

[TestClass]
public class RomanNumerals_test
{
    #region Passes

    [TestMethod]
    public void Pass_1()
    {
        var code = ReadCodeFile("RomanNumerals_1.elan");
        var objectCode = ReadCodeFile("RomanNumerals_1.obj");
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MDCCLXXXIX\r\n");
    }

    [TestMethod]
    public void Pass_2()
    {
        var code = ReadCodeFile("RomanNumerals_2.elan");
        var objectCode = ReadCodeFile("RomanNumerals_2.obj");
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MLXVI\r\n");
    }

    [TestMethod]
    public void Pass_3()
    {
        var code = ReadCodeFile("RomanNumerals_3.elan");
        var objectCode = ReadCodeFile("RomanNumerals_3.obj");
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_4()
    {
        var code = ReadCodeFile("RomanNumerals_4.elan");
        var objectCode = ReadCodeFile("RomanNumerals_4.obj");
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_5()
    {
        var code = ReadCodeFile("RomanNumerals_5.elan");
        var objectCode = ReadCodeFile("RomanNumerals_5.obj");
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_6()
    {
        var code = ReadCodeFile("RomanNumerals_6.elan");
        var objectCode = ReadCodeFile("RomanNumerals_6.obj");
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_7()
    {
        var code = ReadCodeFile("RomanNumerals_7.elan");
        var objectCode = ReadCodeFile("RomanNumerals_7.obj");
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_8()
    {
        var code = ReadCodeFile("RomanNumerals_8.elan");
        var objectCode = ReadCodeFile("RomanNumerals_8.obj");
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MDCCLXXXIX\r\n");
    }

    #endregion

    #region Fails

    #endregion
}