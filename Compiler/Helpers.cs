namespace Compiler;

public static class Helpers {
    public static string GetCsProjForFile(string baseName) =>
        @$"
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<OutputType>Exe</OutputType>
		<TargetFramework>net7.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<Nullable>enable</Nullable>
		<AssemblyName>{baseName}</AssemblyName>
	</PropertyGroup>

    <ItemGroup>
	   <CSFile Include=""{baseName}.cs""/>
    </ItemGroup>

    <ItemGroup>
       <Reference Include=""Sandpit.Compiler.Lib.dll""/>
    </ItemGroup>
</Project>
";
}