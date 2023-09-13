// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace StandardLibrary;

[ElanSystemCall]
public static class SystemCalls {
    public static void printLine(string s) => Console.WriteLine(s);

    public static void printLine(int i) => Console.WriteLine(i);

    public static void printLine(double d) => Console.WriteLine(d);

    public static void printLine(char c) => Console.WriteLine(c);

    public static void printLine(bool b) => Console.WriteLine(b);

    public static void printLine() => Console.WriteLine();
}