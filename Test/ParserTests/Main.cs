namespace Test.ParserTests
{
    [TestClass]
    public class Main
    {
        const string rule = "main";

        [TestMethod]
        public void MainOK()
        {
            var code = @"
main


end main
";
            AssertParsesForRule(code, rule);
        }

        [TestMethod]
        public void MainNoEnd()
        {
            var code = @"
main


";
            var message = "line 5:0 mismatched input '<EOF>' expecting 'end'";
            AssertDoesNotParseForRule(code, rule, message);
        }

        [TestMethod]
        public void MainWrongEnd()
        {
            var code = @"
main

end procedure
";
            var message = "line 4:4 mismatched input 'procedure' expecting 'main'";
            AssertDoesNotParseForRule(code, rule, message);
        }

        [TestMethod]
        public void EndMainNoSpace()
        {
            var code = @"
main

endmain

";
            var message = "line 6:0 mismatched input '<EOF>'";
            AssertDoesNotParseForRule(code, rule, message);
        }


        [TestMethod]
        public void MainWithVar()
        {
            var code = @"
main
var   a  =   1
end main
";
            AssertParsesForRule(code, rule);
        }

        [TestMethod]
        public void MainOfWordle()
        {
            var code = @"
main
   var possible = allPossibleAnswers
   var outcome = """"
   var attempt = """"
   while outcome is not ""*****""
      attempt = bestAttempt(possible, validWords)
      print(attempt)
      outcome = input(""Please mark the attempt"")
      possible = possibleAnswersAfterAttempt(possible, attempt, outcome).asList()
   end while
end main
";
            AssertParsesForRule(code, rule);
        }

        [TestMethod]
        public void MainOfAQAwords()
        {
            var code = @"
main
    print(welcome)
    var player1 = new Player(""Player One"")
    var player2 = new Player(""Player Two"")
    var allowedWords = new List<String>()
    loadAllowedWords(allowedWords)
    var game = new Game(player1, player2, allowedWords)
    var choice = """"
    var randomStart = false
    while choice is not ""9""
        print(mainMenu)
        choice = input(""Enter your choice "")
        randomStart = choice is ""1""
    end while
    if randomStart then
        game.initialiseForRandomStart()
    else
        game.initialiseStandard()
    end if
    play(game)
end main
";
            AssertParsesForRule(code, rule);
        }

    }
}
