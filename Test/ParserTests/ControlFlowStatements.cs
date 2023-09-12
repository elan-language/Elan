namespace Test.ParserTests
{
    [TestClass]
    public class ControlFlowStatements
    {

        [TestMethod]
        public void if1()
        {
            var code = @"
if randomStart then
        x = 1
    else
       x = 2
    end if
";
            AssertParsesForRule(code, "if");
        }

        [TestMethod]
        public void if2()
        {
            var code = @"
        if newTileChoice is ""1"" then
                noOfEndOfTurnTiles = word.length()
        else if newTileChoice is ""2"" then
                noOfEndOfTurnTiles = 3
        else
                noOfEndOfTurnTiles = word.length() + 3
        end if
";
            AssertParsesForRule(code, "if");
        }

        [TestMethod]
        public void forStatement()
        {
            var code = @"
        for count = 0 to startHandSize - 1
            var item = """"
            tileQueue.remove(item)
            hand += item
            tileQueue.add()
        end for
";
            AssertParsesForRule(code, "for");
        }

        [TestMethod]
        public void repeatUntil()
        {
            var code = @"
    repeat
        print(tileChoiceMenu)
        print(""> "")
        newTileChoice = input()
    until tileMenuChoices.contains(newTileChoice)";
            AssertParsesForRule(code, "repeat");
        }

        [TestMethod]
        public void while1()
        {
            var code = @"
while not validChoice
        getChoice(choice)
         if choice is ""1"" then
             print(game.tileValuesAsString())
         else if choice is ""4"" then
             print(game.showTileQueue())
         else if choice is ""7"" then
             print(game.listTileValues(player))
         else if choice is ""0"" then
             validChoice = true
             game.fillHandWithTiles(player)
         else
             validWord = game.checkWordIsInTiles(choice, player)
         end if
         if validWord then
             validWord = game.checkWordIsValid(choice)
             if validWord then
                 print(newline() + ""Valid word"")
                 game.updateAfterAllowedWord(player, choice)
                 newTileChoice = getNewTileChoice()
             end if
         end if
         if not validWord then
              print(""Not a valid attempt, you lose your turn."")
         end if
            if newTileChoice <> ""4""  then
                game.addEndOfTurnTiles(player, newTileChoice, choice)
            end if
            print(""Your word was {choice}"")
            print(""Your new score is {player.score}"")
            print(""You have played {player.tilesPlayed} tiles so far in this game."")
end while
";
            AssertParsesForRule(code, "while");
        }

        [TestMethod]
        public void if3()
        {
            var code = @"
         if choice is ""1"" then
             print(game.tileValuesAsString())
         else if choice is ""4"" then
             print(game.showTileQueue())
         else if choice is ""7"" then
             print(game.listTileValues(player))
         else if choice is ""0"" then
             validChoice = true
             game.fillHandWithTiles(player)
         else
             validWord = game.checkWordIsInTiles(choice, player)
         end if
";
            AssertParsesForRule(code, "if");
        }
    }
}
