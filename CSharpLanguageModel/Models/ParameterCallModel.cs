using System.Runtime.CompilerServices;

namespace CSharpLanguageModel.Models;

public record ParameterCallModel(string Id, bool IsRef = false) : ICodeModel {

    private string IsRefStr => IsRef ? "ref " : ""; 
    
    public string ToString(int indent) => $@"{IsRefStr}{Id}";

    public override string ToString() => ToString(0);
}