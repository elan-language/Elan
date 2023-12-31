﻿namespace CSharpLanguageModel.Models;

public record FuncTypeModel(ICodeModel[] Types, ICodeModel ReturnType) : ICodeModel, IHasDefaultValue {
    public string ToString(int indent) => $"Func<{Types.Append(ReturnType).AsCommaSeparatedString()}>";

    public string DefaultValue => $"({string.Join(",", Types.Select(_ => "_"))}) => {((IHasDefaultValue)ReturnType).DefaultValue}";

    public override string ToString() => ToString(0);
}