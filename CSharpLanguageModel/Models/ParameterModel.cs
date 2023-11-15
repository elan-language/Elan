﻿using System.Runtime.CompilerServices;

namespace CSharpLanguageModel.Models;

public record ParameterModel(ICodeModel Id, ICodeModel Type, bool IsRef = false) : ICodeModel {

    private string IsRefStr => IsRef ? "ref " : ""; 
    
    public string ToString(int indent) => $@"{IsRefStr}{Type} {Id}";

    public override string ToString() => ToString(0);
}