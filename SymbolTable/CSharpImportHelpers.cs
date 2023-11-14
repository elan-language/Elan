using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
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
            _ when type.IsEnum => new EnumSymbolType(type.Name),
            _ => throw new NotImplementedException(type.Name)
        };

    private static bool IsStdLib(MethodInfo m) => m.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null || m.DeclaringType?.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null;

    private static bool IsStdLib(Type m) => m.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null;


    private static bool IsSystemAccessor(MethodInfo m) => m.GetCustomAttribute<ElanSystemAccessorAttribute>() is not null || m.DeclaringType?.GetCustomAttribute<ElanSystemAccessorAttribute>() is not null;

    private static bool IsStatic(this Type t) => t is { IsAbstract: true, IsSealed: true };

    public static void InitTypeSystem(GlobalScope globalScope) {
        var lib = Assembly.GetAssembly(typeof(SystemAccessors));

        var exportedTypes = lib?.ExportedTypes.ToArray() ?? throw new ArgumentException("no lib");

        var allExportedMethods = exportedTypes.Where(t => t.IsStatic()).SelectMany(t => t.GetMethods()).ToList();
        var stdLib = allExportedMethods.Where(IsStdLib).ToArray();

        var systemAccessors = allExportedMethods.Where(IsSystemAccessor).ToArray();
        var constants = exportedTypes.Single(t => t.Name == "Constants").GetFields().ToArray();


        var allExportedClasses = exportedTypes.Where(t => !t.IsStatic()).Where(IsStdLib);


        foreach (var fs in stdLib.Select(methodInfo => new FunctionSymbol(methodInfo.Name, ConvertCSharpTypesToBuiltInSymbol(methodInfo.ReturnType), NameSpace.Library))) {
            globalScope.DefineSystem(fs);
        }

        foreach (var scs in systemAccessors.Select(methodInfo => new SystemAccessorSymbol(methodInfo.Name, ConvertCSharpTypesToBuiltInSymbol(methodInfo.ReturnType), NameSpace.System))) {
            globalScope.DefineSystem(scs);
        }

        foreach (var vs in constants.Select(fieldInfo => new VariableSymbol(fieldInfo.Name, ConvertCSharpTypesToBuiltInSymbol(fieldInfo.FieldType), null!))) {
            globalScope.Define(vs);
        }

        foreach (var vs in allExportedClasses.Select(ImportClass)) {
            globalScope.Define(vs);
        }
    }

    private static ISymbol ImportType(Type type) {
        if (type.IsClass) {
            return ImportClass(type);
        }

        return ImportEnum(type);
    }

    private static ISymbol ImportEnum(Type type) {
        return new EnumSymbol(type.Name, null!);
    }

    private static ISymbol ImportClass(Type type) {
        var cls = new ClassSymbol(type.Name, ClassSymbolTypeType.Mutable, null!);

        var properties = type.GetProperties();

        var methods = type.GetMethods().Where(t => t.DeclaringType == type).Except(properties.SelectMany(p => new[] { p.GetMethod, p.SetMethod })).ToArray();

        var procedures = methods.Where(m => m.ReturnType == typeof(void)).ToArray();
        var functions = methods.Except(procedures).ToArray();

        foreach (var fs in functions.Select(methodInfo => new FunctionSymbol(methodInfo.Name, ConvertCSharpTypesToBuiltInSymbol(methodInfo.ReturnType), cls, NameSpace.UserLocal))) {
            cls.Define(fs);
        }

        foreach (var ps in procedures.Select(methodInfo => new ProcedureSymbol(methodInfo.Name, cls, NameSpace.UserLocal))) {
            cls.Define(ps);
        }

        foreach (var fs in properties.Select(propertyInfo => new VariableSymbol(propertyInfo.Name, ConvertCSharpTypesToBuiltInSymbol(propertyInfo.PropertyType), cls))) {
            cls.Define(fs);
        }

        return cls;
    }
}