using Compiler;

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
  public record class Game {
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {
      p2 = new Player(@$""Chloe"");
      p1 = new Player(@$""Joe"");
      previousScores = new StandardLibrary.ElanList<int>(5, 2, 4);
    }
    public virtual Player p1 { get; private set; } = Player.DefaultInstance;
    public virtual Player p2 { get; private set; } = Player.DefaultInstance;
    public virtual StandardLibrary.ElanList<int> previousScores { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    private record class _DefaultGame : Game {
      public _DefaultGame() { }
      public override Player p1 => Player.DefaultInstance;
      public override Player p2 => Player.DefaultInstance;
      public override StandardLibrary.ElanList<int> previousScores => StandardLibrary.ElanList<int>.DefaultInstance;

      public override string asString() { return ""default Game"";  }
    }
  }
  public record class Player {
    public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
    private Player() {}
    public Player(string name) {
      this.name = name;
    }
    public virtual string name { get; private set; } = """";
    public virtual string asString() {

      return name;
    }
    private record class _DefaultPlayer : Player {
      public _DefaultPlayer() { }
      public override string name => """";

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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue g) = (expression (newInstance (type Game) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . p2)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . previousScores)) ))))) end main) (classDef (mutableClass class Game (constructor constructor ( ) (statementBlock (assignment (assignableValue p2) = (expression (newInstance (type Player) ( (argumentList (expression (value (literal (literalDataStructure ""Chloe""))))) )))) (assignment (assignableValue p1) = (expression (newInstance (type Player) ( (argumentList (expression (value (literal (literalDataStructure ""Joe""))))) )))) (assignment (assignableValue previousScores) = (expression (value (literal (literalDataStructure (literalList { (literal (literalValue 5)) , (literal (literalValue 2)) , (literal (literalValue 4)) }))))))) end constructor) (property property p1 (type Player)) (property property p2 (type Player)) (property property previousScores (type (dataStructureType List (genericSpecifier < (type Int) >)))) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""A game"")))) end function)) end class)) (classDef (mutableClass class Player (constructor constructor ( (parameterList (parameter name (type String))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) name) = (expression (value name)))) end constructor) (property property name (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value name)) end function)) end class)) <EOF>)";
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
  public record class Game {
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {

    }
    public virtual int i { get; private set; } = default;
    public virtual double f { get; private set; } = default;
    public virtual bool b { get; private set; } = default;
    public virtual char c { get; private set; } = default;
    public virtual string s { get; private set; } = """";
    public virtual StandardLibrary.ElanList<int> li { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public virtual StandardLibrary.ElanDictionary<string, int> dsi { get; private set; } = StandardLibrary.ElanDictionary<string, int>.DefaultInstance;
    public virtual StandardLibrary.ElanArray<int> ai { get; private set; } = StandardLibrary.ElanArray<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    private record class _DefaultGame : Game {
      public _DefaultGame() { }
      public override int i => default;
      public override double f => default;
      public override bool b => default;
      public override char c => default;
      public override string s => """";
      public override StandardLibrary.ElanList<int> li => StandardLibrary.ElanList<int>.DefaultInstance;
      public override StandardLibrary.ElanDictionary<string, int> dsi => StandardLibrary.ElanDictionary<string, int>.DefaultInstance;
      public override StandardLibrary.ElanArray<int> ai => StandardLibrary.ElanArray<int>.DefaultInstance;

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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue g) = (expression (newInstance (type Game) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . i)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . f)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . b)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . c)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . s)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . li)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . dsi)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . ai)) ))))) end main) (classDef (mutableClass class Game (constructor constructor ( ) statementBlock end constructor) (property property i (type Int)) (property property f (type Float)) (property property b (type Bool)) (property property c (type Char)) (property property s (type String)) (property property li (type (dataStructureType List (genericSpecifier < (type Int) >)))) (property property dsi (type (dataStructureType Dictionary (genericSpecifier < (type String) , (type Int) >)))) (property property ai (type (dataStructureType Array (genericSpecifier < (type Int) >)))) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""A game"")))) end function)) end class)) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, $"0\r\n0\r\nfalse\r\n{default(char)}\r\n\r\nempty list\r\nempty dictionary\r\nempty array\r\n"); //Not sure if char is correct - use C# default
    }

     [TestMethod]
    public void Pass_DefaultValuesNotPickedUpFromDefaultConstructor()
    {
        var code = @"#
main
    var g = default Game
    printLine(g.i)
end main

class Game
    constructor()
       i = 100
    end constructor

    property i Int

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
  public record class Game {
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {
      i = 100;
    }
    public virtual int i { get; private set; } = default;
    public virtual string asString() {

      return @$""A game"";
    }
    private record class _DefaultGame : Game {
      public _DefaultGame() { }
      public override int i => default;

      public override string asString() { return ""default Game"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var g = Game.DefaultInstance;
    printLine(g.i);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue g) = (expression (value default (type Game)))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . i)) ))))) end main) (classDef (mutableClass class Game (constructor constructor ( ) (statementBlock (assignment (assignableValue i) = (expression (value (literal (literalValue 100)))))) end constructor) (property property i (type Int)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""A game"")))) end function)) end class)) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, $"0\r\n");
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
  public record class Game {
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {

    }
    public virtual Player p1 { get; private set; } = Player.DefaultInstance;
    public virtual Game previousGame { get; private set; } = Game.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    private record class _DefaultGame : Game {
      public _DefaultGame() { }
      public override Player p1 => Player.DefaultInstance;
      public override Game previousGame => Game.DefaultInstance;

      public override string asString() { return ""default Game"";  }
    }
  }
  public record class Player {
    public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
    private Player() {}
    public Player(string name) {
      this.name = name;
    }
    public virtual string name { get; private set; } = """";
    public virtual string asString() {

      return name;
    }
    private record class _DefaultPlayer : Player {
      public _DefaultPlayer() { }
      public override string name => """";

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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue g) = (expression (newInstance (type Game) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . previousGame)) ))))) end main) (classDef (mutableClass class Game (constructor constructor ( ) statementBlock end constructor) (property property p1 (type Player)) (property property previousGame (type Game)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""A game"")))) end function)) end class)) (classDef (mutableClass class Player (constructor constructor ( (parameterList (parameter name (type String))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) name) = (expression (value name)))) end constructor) (property property name (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value name)) end function)) end class)) <EOF>)";
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
  public record class Game {
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {
      score = 1;
    }
    public virtual int score { get; private set; } = default;
    public virtual int best { get; private set; } = default;
    public virtual Player p1 { get; private set; } = Player.DefaultInstance;
    public virtual Player p2 { get; private set; } = Player.DefaultInstance;
    public virtual Game previousGame { get; private set; } = Game.DefaultInstance;
    public virtual StandardLibrary.ElanList<int> previousScores { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    private record class _DefaultGame : Game {
      public _DefaultGame() { }
      public override int score => default;
      public override int best => default;
      public override Player p1 => Player.DefaultInstance;
      public override Player p2 => Player.DefaultInstance;
      public override Game previousGame => Game.DefaultInstance;
      public override StandardLibrary.ElanList<int> previousScores => StandardLibrary.ElanList<int>.DefaultInstance;

      public override string asString() { return ""default Game"";  }
    }
  }
  public record class Player {
    public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
    private Player() {}
    public Player(string name) {
      this.name = name;
    }
    public virtual string name { get; private set; } = """";
    public virtual string asString() {

      return name;
    }
    private record class _DefaultPlayer : Player {
      public _DefaultPlayer() { }
      public override string name => """";

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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue g) = (expression (newInstance (type Game) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value g)) . p1) (binaryOp (conditionalOp is)) (expression (value default (type Player))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value g)) . p2) (binaryOp (conditionalOp is)) (expression (value default (type Player))))) )))) (callStatement (expression (expression (methodCall printLine ( (argumentList (expression (expression (expression (value g)) . previousGame) (binaryOp (conditionalOp is)) (expression (value default (type Game))))) ))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value g)) . previousScores) (binaryOp (conditionalOp is)) (expression (value default (type (dataStructureType List (genericSpecifier < (type Int) >))))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value g)) . score) (binaryOp (conditionalOp is)) (expression (value default (type Int))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value g)) . best) (binaryOp (conditionalOp is)) (expression (value default (type Int))))) ))))) end main) (classDef (mutableClass class Game (constructor constructor ( ) (statementBlock (assignment (assignableValue score) = (expression (value (literal (literalValue 1)))))) end constructor) (property property score (type Int)) (property property best (type Int)) (property property p1 (type Player)) (property property p2 (type Player)) (property property previousGame (type Game)) (property property previousScores (type (dataStructureType List (genericSpecifier < (type Int) >)))) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""A game"")))) end function)) end class)) (classDef (mutableClass class Player (constructor constructor ( (parameterList (parameter name (type String))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) name) = (expression (value name)))) end constructor) (property property name (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value name)) end function)) end class)) <EOF>)";
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
  public record class Game {
    public static Game DefaultInstance { get; } = new Game._DefaultGame();

    public Game() {
      score = 10;
    }
    public virtual int score { get; private set; } = default;
    public virtual int best { get; private set; } = default;
    public virtual Player p1 { get; private set; } = Player.DefaultInstance;
    public virtual Player p2 { get; private set; } = Player.DefaultInstance;
    public virtual Game previousGame { get; private set; } = Game.DefaultInstance;
    public virtual StandardLibrary.ElanList<int> previousScores { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A game"";
    }
    public virtual void setScore(ref int newScore) {
      score = newScore;
    }
    private record class _DefaultGame : Game {
      public _DefaultGame() { }
      public override int score => default;
      public override int best => default;
      public override Player p1 => Player.DefaultInstance;
      public override Player p2 => Player.DefaultInstance;
      public override Game previousGame => Game.DefaultInstance;
      public override StandardLibrary.ElanList<int> previousScores => StandardLibrary.ElanList<int>.DefaultInstance;
      public override void setScore(ref int newScore) { }
      public override string asString() { return ""default Game"";  }
    }
  }
  public record class Player {
    public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
    private Player() {}
    public Player(string name) {
      this.name = name;
    }
    public virtual string name { get; private set; } = """";
    public virtual string asString() {

      return name;
    }
    private record class _DefaultPlayer : Player {
      public _DefaultPlayer() { }
      public override string name => """";

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

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue g) = (expression (newInstance (type Game) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . score)) )))) (callStatement (expression (expression (value g)) . (methodCall setScore ( (argumentList (expression (value default (type Int)))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value g)) . score)) ))))) end main) (classDef (mutableClass class Game (constructor constructor ( ) (statementBlock (assignment (assignableValue score) = (expression (value (literal (literalValue 10)))))) end constructor) (property property score (type Int)) (property property best (type Int)) (property property p1 (type Player)) (property property p2 (type Player)) (procedureDef procedure (procedureSignature setScore ( (parameterList (parameter newScore (type Int))) )) (statementBlock (assignment (assignableValue score) = (expression (value newScore)))) end procedure) (property property previousGame (type Game)) (property property previousScores (type (dataStructureType List (genericSpecifier < (type Int) >)))) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""A game"")))) end function)) end class)) (classDef (mutableClass class Player (constructor constructor ( (parameterList (parameter name (type String))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) name) = (expression (value name)))) end constructor) (property property name (type String)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value name)) end function)) end class)) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n0\r\n");
    }


    [TestMethod]
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
    printLine(f.c is default Dictionary<String,Int>)
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
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {

    }
    public virtual StandardLibrary.ElanList<int> a { get; private set; } = StandardLibrary.ElanList<int>.DefaultInstance;
    public virtual string b { get; private set; } = """";
    public virtual StandardLibrary.ElanDictionary<string, int> c { get; private set; } = StandardLibrary.ElanDictionary<string, int>.DefaultInstance;
    public virtual StandardLibrary.ElanArray<int> d { get; private set; } = StandardLibrary.ElanArray<int>.DefaultInstance;
    public virtual string asString() {

      return @$""A Foo"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override StandardLibrary.ElanList<int> a => StandardLibrary.ElanList<int>.DefaultInstance;
      public override string b => """";
      public override StandardLibrary.ElanDictionary<string, int> c => StandardLibrary.ElanDictionary<string, int>.DefaultInstance;
      public override StandardLibrary.ElanArray<int> d => StandardLibrary.ElanArray<int>.DefaultInstance;

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
    printLine(f.b == """");
    printLine(f.c == StandardLibrary.ElanDictionary<string, int>.DefaultInstance);
    printLine(f.d == StandardLibrary.ElanArray<int>.DefaultInstance);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue f) = (expression (newInstance (type Foo) ( )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . a)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . b)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . c)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value f)) . d)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value f)) . a) (binaryOp (conditionalOp is)) (expression (value default (type (dataStructureType List (genericSpecifier < (type Int) >))))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value f)) . b) (binaryOp (conditionalOp is)) (expression (value default (type String))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value f)) . c) (binaryOp (conditionalOp is)) (expression (value default (type (dataStructureType Dictionary (genericSpecifier < (type String) , (type Int) >))))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value f)) . d) (binaryOp (conditionalOp is)) (expression (value default (type (dataStructureType Array (genericSpecifier < (type Int) >))))))) ))))) end main) (classDef (mutableClass class Foo (constructor constructor ( ) statementBlock end constructor) (property property a (type (dataStructureType List (genericSpecifier < (type Int) >)))) (property property b (type String)) (property property c (type (dataStructureType Dictionary (genericSpecifier < (type String) , (type Int) >)))) (property property d (type (dataStructureType Array (genericSpecifier < (type Int) >)))) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure ""A Foo"")))) end function)) end class)) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "empty list\r\n\r\nempty dictionary\r\nempty array\r\ntrue\r\ntrue\r\ntrue\r\ntrue\r\n");
    }

    #endregion

    #region Fails


    #endregion
}