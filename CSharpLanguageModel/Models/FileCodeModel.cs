namespace CSharpLanguageModel.Models;

public record FileCodeModel(ICodeModel[] Globals, ICodeModel? Main, ICodeModel[] Tests) : ICodeModel {
    public string ToString(int indent) =>
        $@"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {{
{Globals.AsLineSeparatedString(1)}
}}

{Main}{TestCode}";

    public override string ToString() => ToString(0);

    private string TestCode => Tests.Any()
        ? $@"

[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
public class _Tests {{
{Tests.AsLineSeparatedString(1)}
}}"
        : "";
}