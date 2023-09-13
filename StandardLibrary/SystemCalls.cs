// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace StandardLibrary;

[ElanSystemCall]
public static class SystemCalls {
    public static void printLine(string s) => Console.WriteLine(s);

    public static void printLine(int i) => Console.WriteLine(i);
}