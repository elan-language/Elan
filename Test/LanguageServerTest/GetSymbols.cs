using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Server;

namespace Test.LanguageServerTest;

[TestClass]
public class GetSymbols {
    [TestMethod]
    public async Task SimpleGlobals() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  print ""Hello World!""
end main
";

        var buffer = new BufferManager();
        buffer.UpdateBuffer("file:///test1", code);

        var handler = new SymbolHandler(buffer);

        var request = new DocumentSymbolParams { TextDocument = new TextDocumentItem { Uri = new DocumentUri("", "", "test1", "", "") } };

        var container = await handler.Handle(request, new CancellationToken());

        Assert.AreEqual(263, container.Count());
    }
}