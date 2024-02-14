using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Window;
using static Server.Helpers;

namespace Server;

internal class TextDocumentSyncHandler : ITextDocumentSyncHandler {
    private readonly BufferManager _bufferManager;
    private readonly ILanguageServerFacade _router;

    private TextSynchronizationCapability _capability;

    public TextDocumentSyncHandler(ILanguageServerFacade router, BufferManager bufferManager) {
        _router = router;
        _bufferManager = bufferManager;
    }

    public TextDocumentSyncKind Change { get; } = TextDocumentSyncKind.Full;

    public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken) {
        var documentPath = request.TextDocument.Uri.ToString();
        var text = request.ContentChanges.FirstOrDefault()?.Text;

        _bufferManager.UpdateBuffer(documentPath, text);

        _router.Window.LogInfo($"Updated buffer for document: {documentPath}\n{text}");

        return Unit.Task;
    }

    public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken) {
        _bufferManager.UpdateBuffer(request.TextDocument.Uri.ToString(), request.TextDocument.Text);
        return Unit.Task;
    }

    public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken) => Unit.Task;

    public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken) => Unit.Task;

    TextDocumentChangeRegistrationOptions IRegistration<TextDocumentChangeRegistrationOptions, TextSynchronizationCapability>.GetRegistrationOptions(TextSynchronizationCapability capability, ClientCapabilities clientCapabilities) =>
        new() {
            DocumentSelector = ElanDocumentSelector
        };

    TextDocumentOpenRegistrationOptions IRegistration<TextDocumentOpenRegistrationOptions, TextSynchronizationCapability>.GetRegistrationOptions(TextSynchronizationCapability capability, ClientCapabilities clientCapabilities) =>
        new() {
            DocumentSelector = ElanDocumentSelector
        };

    TextDocumentCloseRegistrationOptions IRegistration<TextDocumentCloseRegistrationOptions, TextSynchronizationCapability>.GetRegistrationOptions(TextSynchronizationCapability capability, ClientCapabilities clientCapabilities) =>
        new() {
            DocumentSelector = ElanDocumentSelector
        };

    TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions, TextSynchronizationCapability>.GetRegistrationOptions(TextSynchronizationCapability capability, ClientCapabilities clientCapabilities) =>
        new() {
            DocumentSelector = ElanDocumentSelector,
            IncludeText = true
        };

    public TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri) => new(uri, "elan");

    public TextDocumentChangeRegistrationOptions GetRegistrationOptions() =>
        new() {
            DocumentSelector = ElanDocumentSelector,
            SyncKind = Change
        };

    public void SetCapability(TextSynchronizationCapability capability) {
        _capability = capability;
    }
}