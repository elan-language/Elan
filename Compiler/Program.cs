namespace Compiler;

public static class Program {
    private static void HandleCompile(string fileName) {
        var compileData = Pipeline.Compile(fileName);

        if (compileData.ParserErrors.Length > 0 || compileData.CompileErrors.Length > 0) {
            Console.WriteLine("Failed to Compile");
            foreach (var error in compileData.ParserErrors) {
                Console.WriteLine(error.Message);
            }

            foreach (var error in compileData.CompileErrors) {
                Console.WriteLine(error);
            }
        }
        else {
            Console.WriteLine("Compiled OK");
        }
    }

    private static void Main(string[] args) {
        HandleCompile(args.Single());
    }
}