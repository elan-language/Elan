﻿namespace CSharpLanguageModel.Models;

public record PairModel(ICodeModel Key, ICodeModel Value) : ICodeModel {
    public string ToString(int indent) => $"({Key}, {Value})";
}