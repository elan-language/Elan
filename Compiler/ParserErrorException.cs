using Antlr4.Runtime;

namespace Compiler;

public class ParserErrorException : Exception {
    public ParserErrorException(IRecognizer recognizer,
                                IToken offendingSymbol,
                                int line,
                                int charPositionInLine,
                                string msg,
                                RecognitionException recognitionException) : base($"line {line}:{charPositionInLine} {msg}") { }
}