namespace Test.ParserTests;

[TestClass]
public class Expressions {
    private const string rule = "expression";

    [TestMethod]
    public void SimpleExpression() {
        const string rule = "value";
        var code = @"3";
        AssertParsesForRule(code, rule);
        code = @"foo";
        AssertParsesForRule(code, rule);
        code = "3+";
        AssertDoesNotParseForRule(code, rule, "extraneous input '+'");
    }

    [TestMethod]
    public void IndexedValue() {
        const string rule = "assignableValue";
        var code = @"a[3]";
        AssertParsesForRule(code, rule);
        code = @"b[a]";
        AssertParsesForRule(code, rule);
        code = "b[1..2]";
        AssertParsesForRule(code, rule);
        code = "b[a..b]";
        AssertParsesForRule(code, rule);
        code = "b[a..]";
        AssertParsesForRule(code, rule);
        code = "b[..5]";
        AssertParsesForRule(code, rule);
        code = "b[..]";
        AssertDoesNotParseForRule(code, rule);
        code = "b[..] + 2";
        AssertDoesNotParseForRule(code, rule);
    }

    [TestMethod]
    public void UnaryOp() {
        const string rule = "unaryOp";
        var code = "-";
        AssertParsesForRule(code, rule);
        code = "not";
        AssertParsesForRule(code, rule);
        code = "+";
        AssertDoesNotParseForRule(code, rule, "line 1:0 mismatched input '+'");
        code = "not b";
        AssertDoesNotParseForRule(code, rule, "extraneous input 'b'");
    }

    [TestMethod]
    public void IfExpression1() {
        const string rule = "ifExpression";
        var code = @"if foo then bar else yon";
        AssertParsesForRule(code, rule);
    }

    [TestMethod]
    public void Expression1() {
        var code = @"   tileMenuChoices.contains(newTileChoice)";
        AssertParsesForRule(code, "expression");
    }

    [TestMethod]
    public void Expression2() {
        var code = @"not a";
        AssertParsesForRule(code, "expression");
    }

    [TestMethod]
    public void TestOfSpecificProblem1() {
        var code = @"x and y.isTrue()";
        AssertParsesForRule(code, "expression");
        AssertParseTreeIs(code, "expression", "(expression (expression (value x)) (binaryOp (logicalOp and)) (expression (expression (value y)) . (methodCall isTrue ( ))))");
    }
}