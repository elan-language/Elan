﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using SymbolTable;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;
using static Server.Helpers;

namespace Server;

internal class SymbolHandler(BufferManager bufferManager) : IDocumentSymbolHandler {
    public Task<SymbolInformationOrDocumentSymbolContainer?> Handle(DocumentSymbolParams request, CancellationToken cancellationToken) {
        return Task.Run(() => {
            var documentPath = request.TextDocument.Uri.ToString();
            var compileData = bufferManager.GetBuffer(documentPath).CompileData;
            var st = compileData.SymbolTable;
            var globalSymbols = SymbolHelpers.GetAllSymbolsFlattened(st.GlobalScope);

            var symbols = globalSymbols.Select(s => new SymbolInformation {
                Name = s.Name,
                Kind = SymbolKind.Null,
                Location = new Location {
                    Uri = request.TextDocument.Uri,
                    Range = new Range(0, 0, 0, 0)
                }
            }).Select(si => new SymbolInformationOrDocumentSymbol(si));

            return new SymbolInformationOrDocumentSymbolContainer(symbols);
        }, cancellationToken)!;
    }

    public DocumentSymbolRegistrationOptions GetRegistrationOptions(DocumentSymbolCapability capability, ClientCapabilities clientCapabilities) =>
        new() {
            DocumentSelector = ElanDocumentSelector
        };
}