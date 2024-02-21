using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CSharpLanguageModel;

public static class CSharpCompiler {
    private const int CompileTimeout = 60000;

    private static string GetCsProjForFile(string baseName) =>
        @$"
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<OutputType>Exe</OutputType>
		<TargetFramework>net7.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<Nullable>enable</Nullable>
		<AssemblyName>{baseName}</AssemblyName>
        <StartupObject>Program</StartupObject>
	</PropertyGroup>

    <ItemGroup>
	    <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""17.5.0"" />
        <PackageReference Include=""MSTest.TestAdapter"" Version=""2.2.10"" />
        <PackageReference Include=""MSTest.TestFramework"" Version=""2.2.10"" />
    </ItemGroup>

    <ItemGroup>
	   <CSFile Include=""{baseName}.cs""/>
    </ItemGroup>

    <ItemGroup>
       <Reference Include=""StandardLibrary.dll""/>
    </ItemGroup>
</Project>
";

    private static Process CreateProcess(string csProjFile, string workingDir) {
        var start = new ProcessStartInfo {
            FileName = "dotnet",
            Arguments = $"build {csProjFile}",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardInputEncoding = Encoding.Default,
            StandardOutputEncoding = Encoding.Default,
            WorkingDirectory = workingDir,
            CreateNoWindow = false,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        return Process.Start(start) ?? throw new NullReferenceException("Process failed to start");
    }

    private static (string, string) SetCompileResults(Process process) {
        using var stdOut = process.StandardOutput;
        using var stdErr = process.StandardError;
        var so = stdOut.ReadToEnd();
        var se = stdErr.ReadToEnd();
        return (so, se);
    }

    private static (string, string) SetCompileFailure(Exception e) => ("", e.Message);

    private static (string, string) Compile(string csProjFile, string workingDir) {
        try {
            using var process = CreateProcess(csProjFile, workingDir);

            if (!process.WaitForExit(CompileTimeout)) {
                process.Kill();
            }

            return SetCompileResults(process);
        }
        catch (Exception e) {
            return SetCompileFailure(e);
        }
    }

    public static (string, string, string) CompileObjectCode(string fileName, string objectCode, string workingDir) {
        if (string.IsNullOrWhiteSpace(fileName)) {
            fileName = $"{Guid.NewGuid()}.elan";
        }

        var baseName = Path.GetFileNameWithoutExtension(fileName);
        var dir =  string.IsNullOrEmpty(workingDir) ? Directory.GetCurrentDirectory() : workingDir;

        var objectDir = $@"{dir}\obj\";

        Directory.CreateDirectory(objectDir);

        var files = Directory.GetFiles(objectDir, "*.cs");

        foreach (var file in files) {
            File.Delete(file);
        }

        File.WriteAllText($"{objectDir}{baseName}.cs", objectCode);

        var csproj = GetCsProjForFile(baseName);
        var csProjName = $"{objectDir}{baseName}.csproj";

        File.WriteAllText(csProjName, csproj);

        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location);
        var libPath = Path.Combine(assemblyPath!, "StandardLibrary.dll");

        File.Copy(libPath, $@"{objectDir}\StandardLibrary.dll", true);

        var (stdOut, stdErr) = Compile($"{baseName}.csproj", objectDir);

        return (fileName, stdOut, stdErr);
    }
}