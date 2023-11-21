using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T36_ConditionalExpressions {
    #region Passes

    [TestMethod]
    public void Pass_Filter() {
        var code = @"
main
 print grade(90)
 print grade(70)
 print grade(50)
 print grade(30)
end main

function grade(score Int) as String -> if score > 80 then ""Distinction"" 
  else if score > 60 then ""Merit""
    else if score > 40 then ""Pass""
      else ""Fail""
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Distinction\r\nMerit\r\nPass\r\nFail");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Pass_FailEndIf() {
        var code = @"
main
 print grade(90)
 print grade(70)
 print grade(50)
 print grade(30)
end main

function grade(score Int) as String -> if score > 80 then ""Distinction"" 
  else if score > 60 then ""Merit""
    else if score > 40 then ""Pass""
      else ""Fail""
        end if
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Pass_IfSubClause() {
        var code = @"
main
 print grade(90)
 print grade(70)
 print grade(50)
 print grade(30)
end main

function grade(score Int) as String -> if score > 80 then ""Distinction"" 
   if score > 60 then ""Merit""
      else ""Fail""
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Pass_NoThen() {
        var code = @"
main
 print grade(90)
 print grade(70)
 print grade(50)
 print grade(30)
end main

function grade(score Int) as String -> if score > 80  ""Distinction"" 
      else ""Fail""
        end if
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}