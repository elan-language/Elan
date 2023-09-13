using Antlr4.Runtime;

namespace Compiler;

public class ErrorListener : BaseErrorListener {
    public IList<ParserErrorException> SyntaxErrors { get; } = new List<ParserErrorException>();

    public override void SyntaxError(TextWriter output,
                                     IRecognizer recognizer,
                                     IToken offendingSymbol,
                                     int line,
                                     int charPositionInLine,
                                     string msg,
                                     RecognitionException e) {
        SyntaxErrors.Add(new ParserErrorException(recognizer, offendingSymbol, line, charPositionInLine, msg, e));
    }
}