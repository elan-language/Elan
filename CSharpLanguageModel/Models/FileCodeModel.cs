namespace CSharpLanguageModel.Models;

public record FileCodeModel(IEnumerable<ICodeModel> Globals, ICodeModel? Main) : ICodeModel {
    public string ToString(int indent) =>
        $@"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {{
{Globals.AsLineSeparatedString(1)}
}}

{Main}";

    public override string ToString() => ToString(0);
}