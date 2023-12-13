using System.Xml.Serialization;
using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T93_PassingLoopVariableIntoProcedure {
    #region Passes

    [TestMethod]
    public void Pass_CorrectedPattern() {
        var code = @"#
procedure removeLetters(wordAsPlayed String)
  each letter in wordAsPlayed
    var x set to letter
    call removeLetter(x)
  end each
end procedure

procedure removeLetter(l Char)
end procedure

main
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void removeLetters(string wordAsPlayed) {
    foreach (var letter in wordAsPlayed) {
      var x = letter;
      Globals.removeLetter(x);
    }
  }
  public static void removeLetter(char l) {

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
    public void Fail_InvalidPattern() {
        var code = @"#
procedure removeLetters(wordAsPlayed String)
  each letter in wordAsPlayed
    call removeLetter(letter)
  end each
end procedure

procedure removeLetter(out l Char)
end procedure

main
end main
";
        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void removeLetters(string wordAsPlayed) {
    foreach (var letter in wordAsPlayed) {
      Globals.removeLetter(ref letter);
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
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}