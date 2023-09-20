﻿namespace CSharpLanguageModel.Models;

public record FileCodeModel(IEnumerable<ICodeModel> Globals, ICodeModel? Main) : ICodeModel {

    public override string ToString() => ToString(0);
    public string ToString(int indent) =>
        $@"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {{
{Globals.AsLineSeparatedString(1)}
}}

{Main}";
}