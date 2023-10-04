﻿namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record ProcedureCallModel(ICodeModel Id, IEnumerable<(ICodeModel, bool)> Parameters) : ICodeModel {
    private static string Prefix(bool byRef) =>  byRef ? "ref " : "";

    private static string AsCommaSeparatedString(IEnumerable<(ICodeModel, bool)> mm) => string.Join(", ", mm.Select(v => $"{Prefix(v.Item2)}{v.Item1}"));


    private string GeneratedId(int i) => $"_{Id}_{i}";

    private string GenerateVariables(int indent) {
        var toCreate = Parameters.Select((p, i) => p.Item2 ? "" : $"var {GeneratedId(i)} = {p.Item1}").Where(s => !string.IsNullOrEmpty(s)).ToArray();
        
        return toCreate.Any() ? $"{string.Join("\r\n", toCreate.Select(s => $"{Indent(indent)}{s};"))}\r\n" : "";
    }


    public string ToString(int indent) {
        var parameters = Parameters.Select((p, i) => p.Item2 ? p.Item1 : new ScalarValueModel(GeneratedId(i))); 

        return $@"{GenerateVariables(indent)}{Indent(indent)}{Id}({parameters.AsCommaSeparatedString("ref")})";
    }

    public override string ToString() => ToString(0);
}