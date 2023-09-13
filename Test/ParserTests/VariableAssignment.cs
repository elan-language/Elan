namespace Test.ParserTests; 

[TestClass]
public class VariableAssignment {
    private const string file = "file";

    [TestMethod]
    public void HappyCase() {
        var code = @"
main
  var a = 0
  a = 1
end main
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void TupleDeconstruction() {
        var code = @"
main
  var tup = (3,4)
  var (x,y) = tup
end main
";
        AssertParsesForRule(code, file);
    }

    #region Non-syntactic errors - pending implementation further layers of compile

    [TestMethod]
    public void AssignmentToNonNameNotInScope() {
        var code = @"
main
  a = 4.1
end main
";
        AssertDoesNotParseForRule(code, file, @"");
    }

    [TestMethod]
    public void AssignmentOfWrongType() {
        var code = @"
main
  var a = 0
  a = 4.1
end main
";
        AssertDoesNotParseForRule(code, file, @"");
    }

    [TestMethod]
    public void AssignmentToConstant() {
        var code = @"
main
  constant a = 3.141
  a = 4.0
end main
";
        AssertDoesNotParseForRule(code, file, @"");
    }

    #endregion
}