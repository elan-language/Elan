namespace Test.ParserTests
{
    [TestClass]
    public class LexerTests
    {

        [TestMethod, Ignore("Failing")]
        public void keywordWithinIdentifierIsOk()
        {
            var code = @"
var a = let1";
            AssertParsesForRule(code, "varDef");
            AssertParseTreeIs(code, @"varDef", @"(varDef \r\n var a = (expression (value let1)))");
        }
    }
}
