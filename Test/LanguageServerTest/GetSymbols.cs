using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Server;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Test.LanguageServerTest;

[TestClass]
public class GetSymbols {
    [TestMethod]
    public async Task SimpleGlobalsAsync() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
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

    [TestMethod, Ignore]
    public async Task GoToDeclationAsync() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
constant a set to true
main
  print a
end main
";

        var buffer = new BufferManager();
        buffer.UpdateBuffer("file:///test1", code);

        var handler = new DeclarationHandler(buffer);

        var request = new DeclarationParams() {
            TextDocument = new TextDocumentItem { Uri = new DocumentUri("", "", "test1", "", "") },
            Position = new Position(4, 9)
        };

        var container = await handler.Handle(request, new CancellationToken());
        var location = container?.Single().Location;

        Assert.IsNotNull(location);
        Assert.AreEqual(new Range(new Position(2, 10), new Position(2, 10)), location.Range);
    }
}