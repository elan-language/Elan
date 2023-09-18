namespace Test.ParserTests; 

[TestClass]
public class Constants {
    private const string file = "file";

    [TestMethod]
    public void HappyCaseList() {
        var code = @"
constant allPossibleAnswers = {""ABACK"" }";
        AssertParsesForRule(code, "constantDef");
    }

    [TestMethod]
    public void ConstantDefinedInsideMain()
    {
        var code = @"
main
constant foo = 3
end main";

        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCase() {
        var code = @"
constant x = 3.4
constant y = ""hello""
constant z = {2,3,5,7,11,13,17}
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void UsingAnUnknownNamedValueInAConstant() {
        var code = @"
constant x = 3.4 + a
constant y = ""hello""
constant z = {2,3,5,7,11,13,17}
";
        AssertDoesNotParseForRule(code, file, @""); //TODO should throw an error
    }
}