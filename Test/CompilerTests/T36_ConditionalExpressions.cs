using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T36_ConditionalExpressions {
    #region Passes

    [TestMethod]
    public void Pass_InFunction() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
 print grade(90)
 print grade(70)
 print grade(50)
 print grade(30)
end main

function grade(score Int) as String -> 
  if score > 80
  then ""Distinction"" 
  else if score > 60
  then ""Merit""
  else if score > 40 
  then ""Pass""
  else ""Fail""
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static string grade(int score) {

    return score > 80 ? @$""Distinction"" : score > 60 ? @$""Merit"" : score > 40 ? @$""Pass"" : @$""Fail"";
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.grade(90)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.grade(70)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.grade(50)));
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.grade(30)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Distinction\r\nMerit\r\nPass\r\nFail\r\n");
    }

    [TestMethod]
    public void Pass_InVariableDeclaration() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var score set to 70
  var grade set to if score > 80
      then ""Distinction"" 
      else if score > 60
      then ""Merit""
      else if score > 40 
      then ""Pass""
      else ""Fail""
  print grade
end main

";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var score = 70;
    var grade = score > 80 ? @$""Distinction"" : score > 60 ? @$""Merit"" : score > 40 ? @$""Pass"" : @$""Fail"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(grade));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Merit\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_EndIf() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
 print grade(90)
 print grade(70)
 print grade(50)
 print grade(30)
end main

function grade(score Int) as String ->
    if score > 80
    then ""Distinction"" 
    else if score > 60
    then ""Merit""
    else if score > 40 
    then ""Pass""
    else ""Fail""
    end if
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_IfSubClause() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
 print grade(90)
 print grade(70)
 print grade(50)
 print grade(30)
end main

function grade(score Int) as String -> 
   if score > 80 
   then ""Distinction"" 
   if score > 60 
   then ""Merit""
   else ""Fail""
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoThen() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
 print grade(90)
 print grade(70)
 print grade(50)
 print grade(30)
end main

function grade(score Int) as String -> 
   if score > 80  
   ""Distinction"" 
   else ""Fail""
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}