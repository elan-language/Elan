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
            "Object" => new ClassSymbolType(type.Name),
            "ITuple" => new TupleSymbolType(type.GetGenericArguments().Select(ConvertCSharpTypesToBuiltInSymbol).ToArray()),
            "ElanDictionary`2" => new DictionarySymbolType(),
            "ElanArray`1" => new ArraySymbolType(),
            "ElanList`1" => new ListSymbolType(ConvertCSharpTypesToBuiltInSymbol(type.GetGenericArguments().Single())),
            "IEnumerable`1" => new IterSymbolType(),
            _ when type.Name.StartsWith("Func") => new LambdaSymbolType(type.GetGenericArguments().Select(ConvertCSharpTypesToBuiltInSymbol).SkipLast(1).ToArray(), type.GetGenericArguments().Select(ConvertCSharpTypesToBuiltInSymbol).Last()), 
            _ when type.IsGenericParameter => new GenericSymbolType(type.Name),
            _ when type.IsEnum => new EnumSymbolType(type.Name),
            _ => new ClassSymbolType(type.Name) // placeholder for everything else 
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

        foreach (var fs in stdLib.Where(mi => mi.DeclaringType == typeof(Functions)).Select(mi => CreateFunctionSymbol(mi))) {
            globalScope.DefineSystem(fs);
        }

        foreach (var fs in stdLib.Where(mi => mi.DeclaringType == typeof(Procedures)).Select(mi => CreateProcedureSymbol(mi))) {
            globalScope.DefineSystem(fs);
        }

        foreach (var scs in systemAccessors.Select(mi => CreateSystemAccessorSymbol(mi))) {
            globalScope.DefineSystem(scs);
        }

        foreach (var vs in constants.Select(fieldInfo => CreateVariableSymbol(fieldInfo))) {
            globalScope.Define(vs);
        }

        foreach (var vs in allExportedClasses.Select(ImportType)) {
            globalScope.Define(vs);
        }
    }

    private static ISymbol ImportType(Type type) {
        return type.IsClass ? ImportClass(type) : ImportEnum(type);
    }

    private static ISymbol ImportEnum(Type type) => new EnumSymbol(type.Name, null!);

    private static string[] ParameterIds(MethodInfo mi) => mi.GetParameters().Select(p => p.Name!).ToArray();

    private static void ImportParameters(IScope scope, MethodInfo mi) {
        var pi = mi.GetParameters();
        var ps = pi.Select(p => new ParameterSymbol(p.Name!, ConvertCSharpTypesToBuiltInSymbol(p.ParameterType), p.ParameterType.IsByRef, scope));

        foreach (var p in ps) {
            scope.Define(p);
        }
    }

    private static ISymbol ImportClass(Type type) {
        var cls = new ClassSymbol(type.Name, ClassSymbolTypeType.Mutable, null!);

        var properties = type.GetProperties();

        var methods = type.GetMethods().Where(t => t.DeclaringType == type).Except(properties.SelectMany(p => new[] { p.GetMethod, p.SetMethod })).ToArray();

        var procedures = methods.Where(m => m?.ReturnType == typeof(void)).ToArray();
        var functions = methods.Except(procedures).ToArray();

        foreach (var fs in functions.Select(mi => CreateFunctionSymbol(mi!, cls))) {
            cls.Define(fs);
        }

        foreach (var ps in procedures.Select(mi => CreateProcedureSymbol(mi!, cls))) {
            cls.Define(ps);
        }

        foreach (var fs in properties.Select(propertyInfo => CreateVariableSymbol(propertyInfo, cls))) {
            cls.Define(fs);
        }

        return cls;
    }

    private static ProcedureSymbol CreateProcedureSymbol(MethodInfo mi, IScope? scope = null) {
        var ps = new ProcedureSymbol(mi.Name, scope == null ? NameSpace.LibraryProcedure : NameSpace.UserLocal, ParameterIds(mi), scope);
        ImportParameters(ps, mi);
        return ps;
    }

    private static VariableSymbol CreateVariableSymbol(FieldInfo fi, IScope? scope = null) => new(fi.Name, ConvertCSharpTypesToBuiltInSymbol(fi.FieldType), scope);

    private static VariableSymbol CreateVariableSymbol(PropertyInfo pi, IScope? scope = null) => new(pi.Name, ConvertCSharpTypesToBuiltInSymbol(pi.PropertyType), scope);

    private static SystemAccessorSymbol CreateSystemAccessorSymbol(MethodInfo mi, IScope? scope = null) {
        var sa = new SystemAccessorSymbol(mi.Name, ConvertCSharpTypesToBuiltInSymbol(mi.ReturnType), NameSpace.System, ParameterIds(mi), scope);
        ImportParameters(sa, mi);
        return sa;
    }

    private static (bool, int, int) MatchParameterAtDepth(Type t, Type parameterType, int index, int depth) {

        if (parameterType.Name == t.Name) {
            return (true, index, depth);
        }

        if (parameterType.IsGenericType) {
            var matches = parameterType.GetGenericArguments().Select(pt => MatchParameterAtDepth(t, pt, index, depth + 1));

            return matches.FirstOrDefault(m => m.Item1);
        }

        return (false, index, 0);
    }

    private static (int, int) MatchParametersAtDepth(Type t, Type[] parameterTypes, int depth) {
        return parameterTypes.Select((pt, i) => MatchParameterAtDepth(t, pt, i, 0)).Where(m => m.Item1).Select(tt => (tt.Item2, tt.Item3)).First();
    }

    private static FunctionSymbol CreateFunctionSymbol(MethodInfo mi, IScope? scope = null) {
        var mm = mi.IsGenericMethodDefinition
            ? mi.GetGenericArguments().Select(gp => (gp.Name, MatchParametersAtDepth(gp, mi.GetParameters().Select(p => p.ParameterType).ToArray(), 0))).ToArray()
            : Array.Empty<(string, (int, int))>();

        var fs = mm.Any()
            ? new GenericFunctionSymbol(mi.Name, ConvertCSharpTypesToBuiltInSymbol(mi.ReturnType), scope == null ? NameSpace.LibraryFunction : NameSpace.UserLocal, ParameterIds(mi), mm.ToDictionary(t => t.Item1, t => t.Item2), scope)
            : new FunctionSymbol(mi.Name, ConvertCSharpTypesToBuiltInSymbol(mi.ReturnType), scope == null ? NameSpace.LibraryFunction : NameSpace.UserLocal, ParameterIds(mi), scope);

        ImportParameters(fs, mi);
        return fs;
    }
}