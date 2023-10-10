namespace Test.ParserTests;

[TestClass, Ignore]
public class SystemCalls {
    private const string file = "file";

    [TestMethod]
    public void HappyCaseInputToNewVar() {
        var code = @"
main 
  var choice = input()
end main
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCaseTypedInput() {
        var code = @"
main 
  var choice = input<Decimal>()
end main
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCaseInputToExistingVar() {
        var code = @"
main
  choice = input()
end main";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void AddedArgument() {
        var code = @"
main
  var choice = input(""your choice"")
end main";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void IncompatibleType() {
        var code = @"
main
  var choice = 0
  choice = input()
end main";
        AssertDoesNotParseForRule(code, file, @""); //Should be an error
    }

    [TestMethod]
    public void IncompatibleType2() {
        var code = @"
main
  var choice = """"
  choice = input<Decimal>()
end main";
        AssertDoesNotParseForRule(code, file, @""); //Should be an error
    }

    [TestMethod]
    public void HappyCaseCompatibleType() {
        var code = @"
main
  var choice = 0
  choice = inputint
end main";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCaseRandom() {
        var code = @"
main
  var rnd = new Random()
  var n = rnd.next()
end main";
        AssertParsesForRule(code, file);
    }
}