﻿namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record IdentifierModel(string Id) : ICodeModel {
    public string ToString(int indent) => $"{Indent(indent)}{Prefix(Id)}";

    public override string ToString() => ToString(0);
}