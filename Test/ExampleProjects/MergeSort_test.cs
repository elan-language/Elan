using Compiler;
using Test.CompilerTests;

namespace Test.ExampleProjects;

using static Helpers;

[TestClass]
public class MergeSort_test
{
    #region Passes

    [TestMethod]
    public void Pass_MergeSort_Recursive()
    {
        var code = ReadCodeFile("mergeSort.elan");

        var objectCode = ReadCodeFile("mergeSort.obj");

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }

    #endregion

    #region Fails

    #endregion
}