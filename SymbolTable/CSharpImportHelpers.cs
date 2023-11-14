using System.Reflection;
using StandardLibrary;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace SymbolTable;

public static class CSharpImportHelpers {


    private static ISymbolType ConvertCSharpTypesToBuiltInSymbol(Type type) =>
        type.Name switch {
            "Void" => VoidSymbolType.Instance,
            "String" => StringSymbolType.Instance,
            "Double" => FloatSymbolType.Instance,
            "Int32" => IntSymbolType.Instance,
            "Char" => CharSymbolType.Instance,
            "Boolean" => BoolSymbolType.Instance,
            "IEnumerable`1" => new IterableSymbolType(),
            "ElanDictionary`2" => new DictionarySymbolType(),
            "ElanArray`1" => new ArraySymbolType(),
            "ElanList`1" => new ListSymbolType(),
            _ when type.IsGenericParameter => new GenericSymbolType(),
            _ => throw new NotImplementedException(type.Name)
        };

    private static bool IsStdLib(MethodInfo m) => m.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null || m.DeclaringType?.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null;

    private static bool IsSystemCall(MethodInfo m) => m.GetCustomAttribute<ElanSystemCallAttribute>() is not null || m.DeclaringType?.GetCustomAttribute<ElanSystemCallAttribute>() is not null;

    public static void InitTypeSystem(GlobalScope globalScope) {
        var lib = Assembly.GetAssembly(typeof(SystemCalls));

        var exportedTypes = lib?.ExportedTypes.ToArray() ?? throw new ArgumentException("no lib");

        var allExportedMethods = exportedTypes.SelectMany(t => t.GetMethods()).ToList();

        var stdLib = allExportedMethods.Where(IsStdLib).ToArray();
        var systemCalls = allExportedMethods.Where(IsSystemCall).ToArray();
        var constants = exportedTypes.Single(t => t.Name == "Constants").GetFields().ToArray();

        foreach (var fs in stdLib.Select(methodInfo => new FunctionSymbol(methodInfo.Name, ConvertCSharpTypesToBuiltInSymbol(methodInfo.ReturnType), NameSpace.Library))) {
            globalScope.DefineSystem(fs);
        }

        foreach (var scs in systemCalls.Select(methodInfo => new SystemCallSymbol(methodInfo.Name, ConvertCSharpTypesToBuiltInSymbol(methodInfo.ReturnType), NameSpace.System))) {
            globalScope.DefineSystem(scs);
        }

        foreach (var vs in constants.Select(fieldInfo => new VariableSymbol(fieldInfo.Name, ConvertCSharpTypesToBuiltInSymbol(fieldInfo.FieldType), null!))) {
            globalScope.Define(vs);
        }
    }
}