using System.Collections.Concurrent;
using System.Collections.Generic;
using Compiler;

namespace Server;

internal class BufferManager {
    private readonly ConcurrentDictionary<string, (string Code, CompileData CompileData)> buffers = new();

    public void UpdateBuffer(string documentPath, string buffer) {
        var compileData = Pipeline.Compile(new CompileData { ElanCode = buffer, CompileOptions = new CompileOptions { CompileToCSharp = false } });

        buffers.AddOrUpdate(documentPath, k => (buffer, compileData), (k, v) => (buffer, compileData));
    }

    public (string Code, CompileData CompileData) GetBuffer(string documentPath) => buffers.GetValueOrDefault(documentPath);
}