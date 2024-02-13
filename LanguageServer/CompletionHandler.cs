using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Server
{
    internal class CompletionHandler : ICompletionHandler
    {
        public Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken) {
            return Task.Run(() => {
                    var item1 = new CompletionItem { Label = "Elan", Kind = CompletionItemKind.Text };
                    var item2 = new CompletionItem { Label = "Elan", Kind = CompletionItemKind.Text };

                    return new CompletionList(item1, item2);
                }
            );
        }

        public CompletionRegistrationOptions GetRegistrationOptions(CompletionCapability capability, ClientCapabilities clientCapabilities) {
            return new CompletionRegistrationOptions();
        }
    }
}
