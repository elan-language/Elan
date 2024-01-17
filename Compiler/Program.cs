namespace Compiler;

public static class Program {
    private static void HandleCompile(string fileName, string workingDir) {
        var compileData = Pipeline.Compile(fileName, workingDir);

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
        var fileName = args.First();
        var workingDir = args.Length > 1 ? args[1] : "";

        HandleCompile(fileName, workingDir);
    }
}