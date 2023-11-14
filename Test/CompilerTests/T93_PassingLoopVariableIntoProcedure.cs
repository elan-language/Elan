using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T93_PassingLoopVariableIntoProcedure
{
    #region Passes


    [TestMethod]
    public void Pass_CorrectedPattern()
    {
        var code = @"#
procedure removeLetters(wordAsPlayed String)
  foreach letter in wordAsPlayed
    var x = letter
    call removeLetter(x)
  end foreach
end procedure

procedure removeLetter(l Char)
end procedure

main
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void removeLetters(ref string wordAsPlayed) {
    foreach (var letter in wordAsPlayed) {
      var x = letter;
      removeLetter(ref x);
    }
  }
  public static void removeLetter(ref char l) {

  }
}

public static class Program {
  private static void Main(string[] args) {

  }
}";

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
    [TestMethod]
    public void Fail_InvalidPattern()
    {
        var code = @"#
procedure removeLetters(wordAsPlayed String)
  foreach letter in wordAsPlayed
    call removeLetter(letter)
  end foreach
end procedure

procedure removeLetter(l Char)
end procedure

main
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void removeLetters(ref string wordAsPlayed) {
    foreach (var letter in wordAsPlayed) {
      removeLetter(ref letter);
    }
  }
  public static void removeLetter(ref char l) {

  }
}

public static class Program {
  private static void Main(string[] args) {

  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot pass control variable into a procedure (consider declaring a new variable copying the value)");
    }
    #endregion
}