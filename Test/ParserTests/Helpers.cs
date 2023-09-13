using System.Text.RegularExpressions;
using Antlr4.Runtime;

namespace Test.ParserTests;

public static class Helpers {
    private const string file = "file";

    private static ParserRuleContext GetContext(ElanParser parser, string rule) {
        try {
            var method = parser.GetType().GetMethod(rule);
            return method?.Invoke(parser, null) as ParserRuleContext ?? throw new ArgumentNullException();
        }
        catch (Exception) {
            Assert.Fail($"No such rule {rule} on Parser");
        }

        return null;
    }

    public static void AssertParseTreeIs(string code, string rule, string expectedTree) {
        var actual = TestCode(code, "", rule);
        Assert.AreEqual(expectedTree, actual);
    }

    public static void AssertParsesAsFile(string code) {
        TestCode(code, "", file);
    }

    public static void AssertDoesNotParseAsFile(string code, string message = "*") {
        TestCode(code, message, file);
    }

    public static void AssertParsesForRule(string code, string rule) {
        TestCode(code, "", rule);
    }

    public static void AssertDoesNotParseForRule(string code, string rule, string message = "*") {
        TestCode(code, message, rule);
    }

    public static string ReadInCodeFile(string fileName) => File.ReadAllText($"ParserTests\\{fileName}");

    private static void ThrowIfLeftoverInput(ElanParser parser) {
        //Tests to see if any input left over after rule - if there is and not EOF / whitespace throw exception
        var nextToken = parser.CurrentToken.Text;

        if (!string.IsNullOrWhiteSpace(nextToken) && nextToken != "<EOF>") {
            throw new AggregateException(new Exception($"extraneous input '{nextToken}'"));
        }
    }

    private static string TestCode(string code, string message, string rule, bool ignoreWhiteSpace = true) {
        try {
            var inputStream = new AntlrInputStream(code);
            var lexer = new ElanLexer(inputStream);

            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ElanParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ErrorListener());

            var context = GetContext(parser, rule);

            var parseTree = context.ToStringTree(parser);

            ThrowIfLeftoverInput(parser);

            if (parser.NumberOfSyntaxErrors is 0 && !string.IsNullOrEmpty(message)) {
                Assert.Fail($"Parsed OK but expected error '{message}'");
            }

            if (parser.NumberOfSyntaxErrors > 0) {
                throw new AggregateException(parser.ErrorListeners.OfType<ErrorListener>().First().SyntaxErrors.Cast<Exception>().ToArray());
            }

            return parseTree;
        }
        catch (AggregateException e) {
            if (message == "*") {
                Console.WriteLine($"Wildcard error actual message is '{e.InnerException?.Message}'");
            }
            else {
                var m = e.InnerException?.Message ?? "";
                var actual = ignoreWhiteSpace ? ClearWs(m) : m;
                var expected = ignoreWhiteSpace ? ClearWs(message) : message;
                var choppedActual = expected == "" ? actual : actual.Substring(0, expected.Length);
                Assert.AreEqual(expected, choppedActual, $"Failed with message {choppedActual}");
            }
        }

        return "";
    }

    private static string ClearWs(string inp) => new Regex("\\s+").Replace(inp, " ");

    public class ErrorListener : BaseErrorListener {
        public IList<SyntaxErrorException> SyntaxErrors { get; } = new List<SyntaxErrorException>();

        public override void SyntaxError(TextWriter output,
                                         IRecognizer recognizer,
                                         IToken offendingSymbol,
                                         int line,
                                         int charPositionInLine,
                                         string msg,
                                         RecognitionException e) {
            SyntaxErrors.Add(new SyntaxErrorException(recognizer, offendingSymbol, line, charPositionInLine, msg, e));
        }
    }

    public class SyntaxErrorException : Exception {
        public SyntaxErrorException(IRecognizer recognizer,
                                    IToken offendingSymbol,
                                    int line,
                                    int charPositionInLine,
                                    string msg,
                                    RecognitionException recognitionException) : base($"line {line}:{charPositionInLine} {msg}") { }
    }
}