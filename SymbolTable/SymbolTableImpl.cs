using System.Reflection;
using StandardLibrary;
using SymbolTable.Symbols;

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

    private static bool IsStdLib(MethodInfo m) => m.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null || m.DeclaringType?.GetCustomAttribute<ElanStandardLibraryAttribute>() is not null;

    private static bool IsSystemCall(MethodInfo m) => m.GetCustomAttribute<ElanSystemCallAttribute>() is not null || m.DeclaringType?.GetCustomAttribute<ElanSystemCallAttribute>() is not null;

    private void InitTypeSystem(MethodInfo[] stdLib, MethodInfo[] systemCalls) {
        foreach (var slf in stdLib.Select(sc => new FunctionSymbol(sc.Name))) {
            GlobalScope.Define(slf);
        }

        foreach (var sc in systemCalls.Select(sc => new SystemCallSymbol(sc.Name))) {
            GlobalScope.Define(sc);
        }
    }
}