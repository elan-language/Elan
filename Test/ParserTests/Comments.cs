namespace Test.ParserTests;

[TestClass, Ignore]
public class Comments {
    private const string file = "file";

    [TestMethod]
    public void HappyCase() {
        var code = @"
# first comment
main # comment
# another
# another #
## another #
#without space
  var a = 1 #three
end main # one more
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void CommentWithoutMarker() {
        var code = @"
This is a comment
";
        AssertDoesNotParseForRule(code, file, @"line"); //TODO should throw an error
    }

    [TestMethod]
    public void CommentWithInvalidMarker() {
        var code = @"
// This is a comment
";
        AssertDoesNotParseForRule(code, file, @"line"); //TODO should throw an error
    }
}