namespace Test.ParserTests;

[TestClass]
public class VariableDeclarations {
    private const string file = "file";

    [TestMethod]
    public void HappyCase() {
        var code = @"
main
  var a = 1
end main
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void NoEquals() {
        var code = @"
main
  var a
end main
";
        AssertDoesNotParseForRule(code, file, @"line 3:7 missing '=' at ");
    }

    [TestMethod]
    public void NoValue() {
        var code = @"
main
  var a =
end main
";
        AssertDoesNotParseForRule(code, file, @"line 4:0 no viable alternative");
    }

    #region Non-syntactic errors - pending implementation further layers of compile

    [TestMethod]
    public void DuplicatedVariableName() {
        var code = @"
main
  var a = 3
  var a = 4
end main
";
        AssertDoesNotParseForRule(code, file, @""); //Should fail at compile
    }

    #endregion
}