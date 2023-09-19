// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace StandardLibrary;

[ElanSystemCall]
public static class SystemCalls {
    #region print and input

    //TODO once we can support Any type - replace all overloads with argument of type 'object'
    public static void printLine(string s) => Console.WriteLine(s);

    public static void printLine(int i) => Console.WriteLine(i);

    public static void printLine(double d) => Console.WriteLine(d);

    public static void printLine(char c) => Console.WriteLine(c);

    public static void printLine(bool b) => Console.WriteLine(b);

    public static void printLine() => Console.WriteLine();

    public static string input(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }


    #endregion

    #region random numbers

    public static double random() => new Random().NextDouble();

    public static double random(double max) => random() * max;

    public static double random(double min, double max) => random() * (max - min) + min;

    public static int random(int max) => (int)random((double)max + 1);

    public static int random(int min, int max) => min + random(max - min);

    #endregion
}