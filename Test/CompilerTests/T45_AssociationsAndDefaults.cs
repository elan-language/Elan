using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T45_AssociationsAndDefaults
{
    #region Passes

    [TestMethod]
    public void Pass_CanHavePropertiesThatAreDataStructuresOrObjects()
    {
        var code = @"#
main
    var g = Game()
    printLine(g.p2)
    printLine(g.p1)
    printLine(g.previousScores)
end main

class Game
    constructor()
       p2 = Player(""Chloe"")
       p1 = Player(""Joe"")
       previousScores = {5,2,4}
    end constructor

    property p1 as Player
    property p2 as Player

    property previousScores as List<Int>

    function asString() as String
        return ""A game""
    end function

end class

class Player
    constructor(name String)
        self.name = name
    end constructor

    property name as String

    function asString() as String
        return name
    end function

end class
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public class Game {
    public Game() {
      p2 = new Player(@$""Chloe"");
      p1 = new Player(@$""Joe"");
      previousScores = new StandardLibrary.ElanList<int>(5, 2, 4);
    }
    public Player p1 { get; set; }
    public Player p2 { get; set; }
    public StandardLibrary.ElanList<int> previousScores { get; set; }
    public string asString() {

      return @$""A game"";
    }
  }
  public class Player {
    public Player(string name) {
      this.name = name;
    }
    public string name { get; set; } = """";
    public string asString() {

      return name;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var g = new Game();
    printLine(g.p2);
    printLine(g.p1);
    printLine(g.previousScores);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Chloe\r\nJoe\r\nList {5,2,4}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_PropertiesOfAllStandardTypesHaveDefaultValues()
    {
        var code = @"#
main
    var g = Game()
    printLine(g.i)
    printLine(g.f)
    printLine(g.b)
    printLine(g.c)
    printLine(g.s)
    printLine(g.li)
    printLine(g.dsi)
    printLine(g.ai)
end main

class Game
    constructor()
    end constructor

    property i as Int
    property f as Float
    property b as Boolean
    property c as Char
    property s as String
    property li as List<Int>
    property dsi as Dictionary<String, Int>
    property ai as Array<Int>
   
    function asString() as String
        return ""A game""
    end function

end class

";
        var objectCode = @"";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0\r\n0\r\nfalse\r\n'\\0'\r\n\"\"\r\nList {}\r\nDictionary {}\r\nArray {}"); //Not sure if char is correct - use C# default
    }

    [TestMethod, Ignore]
    public void Pass_PropertiesOfClassTypesHaveDefaultValues()
    {
        var code = @"#
main
    var g = Game()
    printLine(g.p1)
    printLine(g.previousGame)
end main

class Game
    constructor()
    end constructor

    property p1 as Player
    property previousGame as Game

    function asString() as String
        return ""A game""
    end function

end class

class Player
    constructor(name String)
        self.name = name
    end constructor

    property name as String

    function asString() as String
        return name
    end function

end class
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public class Game {
    public Game() {

    }
    public Player p1 { get; set; }
    public Player p2 { get; set; }
    public StandardLibrary.ElanList<int> previousScores { get; set; }
    public string asString() {

      return @$""A game"";
    }
  }
  public class Player {
    public Player(string name) {
      this.name = name;
    }
    public string name { get; set; } = """";
    public string asString() {

      return name;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var g = new Game();
    printLine(g.p1);
    printLine(g.p2);
    printLine(g.previousScores);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "default Player\r\ndefault Game\r\n");
    }

    [TestMethod]
    public void Pass_defaultKeywordToTestValue()
    {
        var code = @"#
main
    var g = Game()
    printLine(g.p1 is default Player)
    printLine(g.p2 is default Player)
    printLine(g.p2 is default Game)
    printLine(g.previousGame is default Game)
    printLine(g.previousScores is default List<Int>)
    printLine(g.score is default Int)
    printLine(g.best is default Int)
end main

class Game
    constructor()
        score = 1
    end constructor

    property score as Int
    property best as Int

    property p1 as Player
    property p2 as Player

    property previousGame as Game

    property previousScores as List<Int>

    function asString() as String
        return ""A game""
    end function

end class

class Player
    constructor(name String)
        self.name = name
    end constructor

    property name as String

    function asString() as String
        return name
    end function

end class
";
        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public class Game {
    public Game() {
      score = 1;
    }
    public int score { get; set; }
    public int best { get; set; }
    public Player p1 { get; set; }
    public Player p2 { get; set; }
    public Game previousGame { get; set; }
    public StandardLibrary.ElanList<int> previousScores { get; set; }
    public string asString() {

      return @$""A game"";
    }
  }
  public class Player {
    public Player(string name) {
      this.name = name;
    }
    public string name { get; set; } = """";
    public string asString() {

      return name;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var g = new Game();
    printLine(g.p1 == Player.DefaultInstance);
    printLine(g.p2 == Player.DefaultInstance);
    printLine(g.p2 == Game.DefaultInstance);
    printLine(g.previousGame == Game.DefaultInstance);
    printLine(g.previousScores == StandardLibrary.ElanList<int>.DefaultInstance);
    printLine(g.score == default(int));
    printLine(g.best == default(int));
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\ntrue\r\ntrue\r\nfalse\r\nfalse\r\n");
    }

    [TestMethod, Ignore]    
    public void Pass_defaultValueCanBeAssigned()
    {
        var code = @"#
main
    var g = Game()
    printLine(g.score)
    g.setScore(default Int)
    printLine(g.score)
end main

class Game
    constructor()
       score = 10
    end constructor

    property score as Int
    property best as Int

    property p1 as Player
    property p2 as Player

    procedure setScore(newScore Int)
        score = newScore
    end procedure

    property previousGame as Game

    property previousScores as List<Int>

    function asString() as String
        return ""A game""
    end function

end class

class Player
    constructor(name String)
        self.name = name
    end constructor

    property name as String

    function asString() as String
        return name
    end function

end class
";
        var objectCode = @"";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n0\r\n");
    }

    #endregion

    #region Fails


    #endregion
}

public class Simple {
    public int P1 { get; set; }
    public void setP1(int value) { P1 = value; }
}

public class EmptySimple : Simple {

}