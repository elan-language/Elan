﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly:InternalsVisibleToAttribute("Test")]

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = await LanguageServer.From(options =>
                options
                    .WithInput(Console.OpenStandardInput())
                    .WithOutput(Console.OpenStandardOutput())
                    .WithLoggerFactory(new LoggerFactory())
                    .AddDefaultLoggingProvider()
                    .WithServices(ConfigureServices)
                    .WithHandler<TextDocumentSyncHandler>()
                    .WithHandler<CompletionHandler>()
                    .WithHandler<SymbolHandler>()
                );

            await server.WaitForExit;
        }

        static void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<BufferManager>();
        }
    }
}
