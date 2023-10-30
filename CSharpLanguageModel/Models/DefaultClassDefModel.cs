﻿namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record DefaultClassDefModel(ICodeModel Type, IEnumerable<ICodeModel> Procedures) : ICodeModel {

    public string ToString(int indent) =>
        $@"{Indent(indent)}private class _Default{Type} : {Type} {{
{DefaultConstructor(indent)}
{Procedures.AsLineSeparatedString("public override ", " { }", indent + 1)}
{Indent(indent + 1)}public override string asString() {{ return ""default {Type}"";  }}
{Indent(indent)}}}";

    private string DefaultConstructor(int indent) => $"{Indent(indent + 1)}public _Default{Type}() {{ }}";

    public override string ToString() => ToString(0);
}