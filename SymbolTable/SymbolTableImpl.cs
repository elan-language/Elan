using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using StandardLibrary;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace SymbolTable;

public class SymbolTableImpl {
    public SymbolTableImpl() {
        var lib = Assembly.GetAssembly(typeof(SystemCalls));

        var sc = lib?.ExportedTypes.ToArray() ?? throw new ArgumentException("no lib");

        var allExportedMethods = sc.SelectMany(t => t.GetMethods()).ToList();

        var stdLib = allExportedMethods.Where(IsStdLib).ToArray();
        var systemCalls = allExportedMethods.Where(IsSystemCall).ToArray();

        InitTypeSystem(stdLib, systemCalls);
    }

    public GlobalScope GlobalScope { get; } = new();

    private static ISymbolType ConvertToBuiltInSymbol(Type type) =>
        type.Name switch {
            "Void" => VoidSymbolType.Instance,
            "String" => StringSymbolType.Instance,
            "Double" => FloatSymbolType.Instance,
            "Int32" => IntSymbolType.Instance,
            "Decimal" => DecimalSymbolType.Instance,
            "Char" => CharSymbolType.Instance,
            "Boolean" => BooleanSymbolType.Instance,
            "IEnumerable`1" => new IterableSymbolType(),
            "IImmutableDictionary`2" => new DictionarySymbolType(),
            "ElanArray`1" => new ArraySymbolType(),
            "ElanList`1" => new ListSymbolType(),
            _ when type.IsGenericParameter => new GenericSymbolType(),
            _ => throw new NotImplementedException(type.Name)
        };

    private static bool IsStdLib(MethodInfo m) => m.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null || m.DeclaringType?.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null;

    private static bool IsSystemCall(MethodInfo m) => m.GetCustomAttribute<ElanSystemCallAttribute>() is not null || m.DeclaringType?.GetCustomAttribute<ElanSystemCallAttribute>() is not null;

    private void InitTypeSystem(MethodInfo[] stdLib, MethodInfo[] systemCalls) {
        foreach (var slf in stdLib.Select(sc => new FunctionSymbol(sc.Name, ConvertToBuiltInSymbol(sc.ReturnType), NameSpace.Library))) {
            GlobalScope.DefineSystem(slf);
        }

        foreach (var sc in systemCalls.Select(sc => new SystemCallSymbol(sc.Name, ConvertToBuiltInSymbol(sc.ReturnType)))) {
            GlobalScope.DefineSystem(sc);
        }
    }
}