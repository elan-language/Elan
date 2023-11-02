using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] 
public class T81_CompletePrograms
{
    #region Passes

    [TestMethod, Ignore]
    public void Pass_BinarySearch() {
         var code = ReadElanSourceCodeFile("binarySearch.elan");

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        
    }

    [TestMethod, Ignore]
    public void Pass_MergeSort()
    {
        var code = ReadElanSourceCodeFile("mergeSort.elan");

        var objectCode = @"";

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