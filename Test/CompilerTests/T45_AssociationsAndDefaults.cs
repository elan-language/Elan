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
    printLine(g.p1)
    printLine(g.p2)
    printLine(g.previousGame)
    printLine(g.previousScores)
end main

class Game
    constructor()
       p2 = Player(""Chloe"")
       p1 = Player(""Joe"")
       previousGame = Game()
       previousScores = {5,2,4}
    end constructor

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
        var objectCode = @"";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Chloe\r\nJoe\r\nA game\r\nList {5,2,4}");
    }

    [TestMethod]
    public void Pass_AllPropertiesHaveDefaultValues()
    {
        var code = @"#
main
    var g = Game()
    printLine(g.p1)
    printLine(g.p2)
    printLine(g.previousGame)
    printLine(g.previousScores)
end main

class Game
    constructor()
    end constructor

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
        var objectCode = @"";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "\"\"\r\n\"\"\r\nA game\r\nList {}");
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
        var objectCode = @"";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\nfalse\r\ntrue\r\ntrue\r\nfalse\r\nfalse\r\n");
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