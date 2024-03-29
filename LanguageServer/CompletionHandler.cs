﻿using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using static Server.Helpers;

namespace Server;

internal class CompletionHandler : ICompletionHandler {
    public Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken) {
        return Task.Run(() => {
            var item1 = new CompletionItem { Label = "Elan", Kind = CompletionItemKind.Text };
            var item2 = new CompletionItem { Label = "Elan", Kind = CompletionItemKind.Text };

            return new CompletionList(item1, item2);
        }, cancellationToken);
    }

    public CompletionRegistrationOptions GetRegistrationOptions(CompletionCapability capability, ClientCapabilities clientCapabilities) =>
        new() {
            DocumentSelector = ElanDocumentSelector
        };
}