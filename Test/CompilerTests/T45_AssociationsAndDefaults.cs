﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
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

    property p1 Player
    property p2 Player

    property previousScores List<Int>

    function asString() -> String
        return ""A game""
    end function

end class

class Player
    constructor(name String)
        self.name = name
    end constructor

    property name String

    function asString() -> String
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
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {
      p2 = new Player(@$""Chloe"");
      p1 = new Player(@$""Joe"");
      previousScores = new StandardLibrary.ElanList<int>(5, 2, 4);
    }
    public Player p1 { get; private set; } = Player.DefaultInstance;
    public Player p2 { get; private set; } = Player.DefaultInstance;
    public StandardLibrary.ElanList<int> previousScores { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    private class _DefaultGame : Game {
      public _DefaultGame() { }

      public override string asString() { return ""default Game"";  }
    }
  }
  public class Player {
    public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
    private Player() {}
    public Player(string name) {
      this.name = name;
    }
    public string name { get; private set; } = """";
    public virtual string asString() {

      return name;
    }
    private class _DefaultPlayer : Player {
      public _DefaultPlayer() { }

      public override string asString() { return ""default Player"";  }
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

    [TestMethod]
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

    property i Int
    property f Float
    property b Bool
    property c Char
    property s String
    property li List<Int>
    property dsi Dictionary<String, Int>
    property ai Array<Int>

    function asString() -> String
        return ""A game""
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
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {

    }
    public int i { get; private set; } = default;
    public double f { get; private set; } = default;
    public bool b { get; private set; } = default;
    public char c { get; private set; } = default;
    public string s { get; private set; } = """";
    public StandardLibrary.ElanList<int> li { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public StandardLibrary.ElanDictionary<string, int> dsi { get; private set; } = StandardLibrary.ElanDictionary<string, int>.DefaultInstance;
    public StandardLibrary.ElanArray<int> ai { get; private set; } = StandardLibrary.ElanArray<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    private class _DefaultGame : Game {
      public _DefaultGame() { }

      public override string asString() { return ""default Game"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var g = new Game();
    printLine(g.i);
    printLine(g.f);
    printLine(g.b);
    printLine(g.c);
    printLine(g.s);
    printLine(g.li);
    printLine(g.dsi);
    printLine(g.ai);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, $"0\r\n0\r\nfalse\r\n{default(char)}\r\n\r\nempty list\r\nempty dictionary\r\nempty array\r\n"); //Not sure if char is correct - use C# default
    }

    [TestMethod]
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

    property p1 Player
    property previousGame Game

    function asString() -> String
        return ""A game""
    end function

end class

class Player
    constructor(name String)
        self.name = name
    end constructor

    property name String

    function asString() -> String
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
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {

    }
    public Player p1 { get; private set; } = Player.DefaultInstance;
    public Game previousGame { get; private set; } = Game.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    private class _DefaultGame : Game {
      public _DefaultGame() { }

      public override string asString() { return ""default Game"";  }
    }
  }
  public class Player {
    public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
    private Player() {}
    public Player(string name) {
      this.name = name;
    }
    public string name { get; private set; } = """";
    public virtual string asString() {

      return name;
    }
    private class _DefaultPlayer : Player {
      public _DefaultPlayer() { }

      public override string asString() { return ""default Player"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var g = new Game();
    printLine(g.p1);
    printLine(g.previousGame);
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
   
    printLine(g.previousGame is default Game)
    printLine(g.previousScores is default List<Int>)
    printLine(g.score is default Int)
    printLine(g.best is default Int)
end main

class Game
    constructor()
        score = 1
    end constructor

    property score Int
    property best Int

    property p1 Player
    property p2 Player

    property previousGame Game

    property previousScores List<Int>

    function asString() -> String
        return ""A game""
    end function

end class

class Player
    constructor(name String)
        self.name = name
    end constructor

    property name String

    function asString() -> String
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
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {
      score = 1;
    }
    public int score { get; private set; } = default;
    public int best { get; private set; } = default;
    public Player p1 { get; private set; } = Player.DefaultInstance;
    public Player p2 { get; private set; } = Player.DefaultInstance;
    public Game previousGame { get; private set; } = Game.DefaultInstance;
    public StandardLibrary.ElanList<int> previousScores { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    private class _DefaultGame : Game {
      public _DefaultGame() { }

      public override string asString() { return ""default Game"";  }
    }
  }
  public class Player {
    public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
    private Player() {}
    public Player(string name) {
      this.name = name;
    }
    public string name { get; private set; } = """";
    public virtual string asString() {

      return name;
    }
    private class _DefaultPlayer : Player {
      public _DefaultPlayer() { }

      public override string asString() { return ""default Player"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var g = new Game();
    printLine(g.p1 == Player.DefaultInstance);
    printLine(g.p2 == Player.DefaultInstance);
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
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\ntrue\r\ntrue\r\nfalse\r\ntrue\r\n");
    }

    [TestMethod]    
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

    property score Int
    property best Int

    property p1 Player
    property p2 Player

    procedure setScore(newScore Int)
        score = newScore
    end procedure

    property previousGame Game

    property previousScores List<Int>

    function asString() -> String
        return ""A game""
    end function

end class

class Player
    constructor(name String)
        self.name = name
    end constructor

    property name String

    function asString() -> String
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
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {
      score = 10;
    }
    public int score { get; private set; } = default;
    public int best { get; private set; } = default;
    public Player p1 { get; private set; } = Player.DefaultInstance;
    public Player p2 { get; private set; } = Player.DefaultInstance;
    public Game previousGame { get; private set; } = Game.DefaultInstance;
    public StandardLibrary.ElanList<int> previousScores { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    public virtual void setScore(ref int newScore) {
      score = newScore;
    }
    private class _DefaultGame : Game {
      public _DefaultGame() { }
      public override void setScore(ref int newScore) { }
      public override string asString() { return ""default Game"";  }
    }
  }
  public class Player {
    public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
    private Player() {}
    public Player(string name) {
      this.name = name;
    }
    public string name { get; private set; } = """";
    public virtual string asString() {

      return name;
    }
    private class _DefaultPlayer : Player {
      public _DefaultPlayer() { }

      public override string asString() { return ""default Player"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var g = new Game();
    printLine(g.score);
    var _setScore_0 = default(int);
    g.setScore(ref _setScore_0);
    printLine(g.score);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n0\r\n");
    }


    [TestMethod, Ignore]
    public void Pass_defaultForStandardDataStructures()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.a)
    printLine(f.b)
    printLine(f.c)
    printLine(f.d)
    printLine(f.a is default List<Int>)
    printLine(f.b is default String)
    printLine(f.c is default Dicionary<String,Int>)
    printLine(f.d is default Array<Int>)
end main

class Foo
    constructor()
    end constructor

    property a List<Int>
    property b String
    property c Dictionary<String, Int>
    property d Array<Int>

    function asString() -> String
        return ""A Foo""
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
  public class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {

    }
    public StandardLibrary.ElanList<int> a { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public string b { get; private set; } = """";
    public StandardLibrary.ElanDictionary<string, int> c { get; private set; } = StandardLibrary.ElanDictionary<string, int>.DefaultInstance;
    public StandardLibrary.ElanArray<int> d { get; private set; } = StandardLibrary.ElanArray<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A Foo"";
    }
    private class _DefaultFoo : Foo {
      public _DefaultFoo() { }

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    printLine(f.a);
    printLine(f.b);
    printLine(f.c);
    printLine(f.d);
    printLine(f.a == StandardLibrary.ElanList<int>.DefaultInstance);
    printLine(f.b == default(string));
    printLine(f.c == Dicionary.DefaultInstance);
    printLine(f.d == StandardLibrary.ElanArray<int>.DefaultInstance);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {}\r\n\"\"\r\nDictionary {}\r\nArray {}\r\ntrue\r\ntrue\r\ntrue\r\ntrue\r\n");
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