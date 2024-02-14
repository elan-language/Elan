using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Server;

internal static class Helpers {
    public static readonly TextDocumentSelector ElanDocumentSelector = new(
        new TextDocumentFilter {
            Pattern = "**/*.elan"
        }
    );
}