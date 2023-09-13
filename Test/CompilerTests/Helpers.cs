using Compiler;

namespace Test.CompilerTests;

public static class Helpers {
    public static void AssertParses(CompileData compileData) {
        Assert.IsTrue(compileData.Parser?.NumberOfSyntaxErrors == 0, "Unexpected parse error");
    }

    public static void AssertCompiles(CompileData compileData) {
        Assert.IsNotNull(compileData.AbstractSyntaxTree, "Failed to compile");
    }

    public static void AssertObjectCodeIs(CompileData compileData, string objectCode) {
        Assert.AreEqual(objectCode, compileData.ObjectCode);
    }

    public static void AssertDoesNotParse(CompileData compileData) {
        Assert.IsTrue(compileData.Parser?.NumberOfSyntaxErrors > 0, "Expected parse error");
    }
}