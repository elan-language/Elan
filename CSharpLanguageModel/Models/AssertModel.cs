namespace CSharpLanguageModel.Models;

using static CodeHelpers;

public record AssertModel(ICodeModel Actual, ICodeModel Expected) : ICodeModel {
    
    public string ToString(int indent) => $"{Indent(indent)}Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual({Expected}, {Actual});";
}