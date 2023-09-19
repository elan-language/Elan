namespace Test.ParserTests;

[TestClass]
public class LexerTests {
    [TestMethod] [Ignore("Failing on appveyor")]
    public void keywordWithinIdentifierIsOk() {
        var code = @"
var let_1 = 3";
        AssertParsesForRule(code, "varDef");
        AssertParseTreeIs(code, @"varDef", @"(varDef \r\n var (assignableValue let_1) = (expression (value (literalValue 3))))");
    }
}