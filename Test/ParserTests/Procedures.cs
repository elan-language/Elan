namespace Test.ParserTests;

[TestClass, Ignore]
public class Procedures {
    private const string file = "file";

    [TestMethod]
    public void HappyCase() {
        var code = @"
procedure getNewTileChoice(newTileChoice String)
    repeat
        print(tileChoiceMenu)
        print(""> "")
        newTileChoice = input()
    until tileMenuChoices.contains(newTileChoice)
end procedure
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCaseNoParams() {
        var code = @"
procedure getNewTileChoice()
end procedure
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void MisspelledProcedure() {
        var code = @"
proceedure getNewTileChoice(newTileChoice String)
    repeat
        print(tileChoiceMenu)
        print(""> "")
        newTileChoice = input
    until tileMenuChoices.contains(newTileChoice)
end procedure
";
        AssertDoesNotParseForRule(code, file, @"line 2:0 no viable alternative"); //How does it know that ?!!
    }

    [TestMethod]
    public void InvalidName() {
        var code = @"
procedure getNew-TileChoice(newTileChoice String)
    repeat
        print(tileChoiceMenu)
        print(""> "")
        newTileChoice = input
    until tileMenuChoices.contains(newTileChoice)
end procedure
";
        AssertDoesNotParseForRule(code, file, @"line 2:16 mismatched input '-' expecting '('");
    }

    [TestMethod]
    public void InvalidCaseName() {
        var code = @"
procedure getNewTileChoice(newTileChoice String)
    repeat
        print(tileChoiceMenu)
        print(""> "")
        newTileChoice = input()
    until tileMenuChoices.contains(newTileChoice)
end procedure
";
        AssertDoesNotParseForRule(code, file, @""); //TODO: case not enforced in parser 
    }

    [TestMethod]
    public void AsClauseAdded() {
        var code = @"
procedure getNewTileChoice(newTileChoice String) as String
    repeat
        print(tileChoiceMenu)
        print(""> "")
        newTileChoice = input
    until tileMenuChoices.contains(newTileChoice)
end procedure
";
        AssertDoesNotParseForRule(code, file, @"line 2:49 mismatched input 'as'");
    }

    [TestMethod]
    public void ReturnAdded() {
        var code = @"
procedure getNewTileChoice(newTileChoice String)
    return  newTileChoice
end procedure
";
        AssertDoesNotParseForRule(code, file, @"line 3:4 mismatched input 'return'");
    }

    [TestMethod]
    public void InnerProcedure() {
        var code = @"
procedure getNewTileChoice(newTileChoice String)
    procedure getOldTileChoice(newTileChoice String)
    end procedure
end procedure
";
        AssertDoesNotParseForRule(code, file, @"line 3:4 missing 'end' at 'procedure'");
    }
}