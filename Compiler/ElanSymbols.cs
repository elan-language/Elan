namespace Compiler;

public static class ElanSymbols {
    public static readonly string NewLine = "\r\n";
    public static readonly string Comment = ElanParser.DefaultVocabulary.GetLiteralName(ElanParser.COMMENT_MARKER).Replace("'", "");
}