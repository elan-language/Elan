namespace CSharpLanguageModel.Models;

public record FileCodeModel(IEnumerable<ICodeModel> Globals, ICodeModel? Main) : ICodeModel {
    public override string ToString() =>
        $@"
using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {{
{Globals.AsLineSeparatedString(1, "public")}
}}

{Main.AsCode()}".Trim();
}