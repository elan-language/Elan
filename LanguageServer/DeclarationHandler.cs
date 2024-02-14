using System;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using static Server.Helpers;

namespace Server;

internal class DeclarationHandler(BufferManager bufferManager) : IDeclarationHandler {
    public Task<LocationOrLocationLinks?> Handle(DeclarationParams request, CancellationToken cancellationToken) {
        return Task.Run(() => {
            var position = request.Position;
            var documentPath = request.TextDocument.Uri.ToString();
            var compileData = bufferManager.GetBuffer(documentPath).CompileData;
            var st = compileData.SymbolTable;
            //var globalSymbols = SymbolHelpers.GetAllSymbolsFlattened(st.GlobalScope);

            //var symbols = globalSymbols.Select(s => new SymbolInformation {
            //    Name = s.Name,
            //    Kind = SymbolKind.Null,
            //    Location = new Location {
            //        Uri = request.TextDocument.Uri,
            //        Range = new Range(0, 0, 0, 0)
            //    }
            //}).Select(si => new SymbolInformationOrDocumentSymbol(si));

            return new LocationOrLocationLinks();
        }, cancellationToken)!;
    }

    public DeclarationRegistrationOptions GetRegistrationOptions(DeclarationCapability capability, ClientCapabilities clientCapabilities) =>
        new() {
            DocumentSelector = ElanDocumentSelector
        };
}