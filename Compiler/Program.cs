namespace Compiler;

public static class Program {
    private static void HandleCompile(string fileName) {
        Pipeline.Compile(fileName);
        Console.WriteLine("Compiled OK");
    }

    private static void Main(string[] args) {
        HandleCompile(args.Single());
    }
}