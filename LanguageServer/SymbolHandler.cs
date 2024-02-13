using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using SymbolTable;

namespace Server
{
    internal class SymbolHandler : IDocumentSymbolHandler
    {
        private readonly BufferManager bufferManager;

        public SymbolHandler(BufferManager bufferManager) {
            this.bufferManager = bufferManager;
        }

        public Task<SymbolInformationOrDocumentSymbolContainer?> Handle(DocumentSymbolParams request, CancellationToken cancellationToken) {
            return Task.Run(() => {
                var documentPath = request.TextDocument.Uri.ToString();
                var compileData = bufferManager.GetBuffer(documentPath).CompileData;
                var st = compileData.SymbolTable;
                var globalSymbols = SymbolHelpers.GetAllSymbolsFlattened(st.GlobalScope);

                var symbols = globalSymbols.Select(s => new SymbolInformation { Name = s.Name }).Select(si => new SymbolInformationOrDocumentSymbol(si));
                return new SymbolInformationOrDocumentSymbolContainer(symbols);
            }, cancellationToken)!;
        }

        public DocumentSymbolRegistrationOptions GetRegistrationOptions(DocumentSymbolCapability capability, ClientCapabilities clientCapabilities) => new();
    }
}
