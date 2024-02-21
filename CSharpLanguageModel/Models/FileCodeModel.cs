namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record FileCodeModel(ICodeModel[] Globals, ICodeModel? Main, ICodeModel[] Tests) : ICodeModel {
    private string TestCode => Tests.Any()
        // must be in namespace or test not found by MsTest
        ? $@"
namespace ElanTestCode {{
{Indent(1)}[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
{Indent(1)}public class _Tests {{
{Tests.AsLineSeparatedString(2)}
{Indent(1)}}}
}}"
        : "";

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
}