﻿namespace CSharpLanguageModel.Models;

public record WithModel(ICodeModel Expression, ICodeModel[] With) : ICodeModel {
    public string ToString(int indent) => $"{Expression} with {{{With.AsCommaSeparatedString()}}}";

    public override string ToString() => ToString(0);
}