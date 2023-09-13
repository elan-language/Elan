namespace CSharpLanguageModel.Models;

public record FileCodeModel(ICodeModel? Main) : ICodeModel {
    public override string ToString() =>
        $@"
using System.Collections.Generic;
using System.Collections.Immutable;
using static StandardLibrary.SystemCalls;

{Main.AsCode()}".Trim();
}