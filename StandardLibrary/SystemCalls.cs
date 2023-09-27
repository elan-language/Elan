// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
using System.ComponentModel;
using static StandardLibrary.Functions;

namespace StandardLibrary;

[ElanSystemCall]
public static class SystemCalls
{
    #region print and input

    //TODO once we can support Any type - replace all overloads with argument of type 'object'
    public static void printLine(object obj) => Console.WriteLine(asString(obj));

    public static void printLine() => Console.WriteLine();

    public static void print(object obj) => Console.Write(asString(obj));

    public static string input(string prompt)
    {
        Console.Write(prompt);
        return input();
    }

    public static string input()
    {
        return Console.ReadLine();
    }
    #endregion

    #region random numbers

    private static object randomLock = new object();

    private static Random generator = new Random();

    public static void seedRandom(int seed)
    {
        lock (randomLock)
        {
            generator = new Random(seed);
        }
    }

    public static double random()
    {
        lock (randomLock)
        {
            return generator.NextDouble();
        }
    }

    public static double random(double max)
    {
        lock (randomLock)
        {
            return random() * max;
        }
    }

    public static double random(double min, double max)
    {
        lock (randomLock)
        {
            return min + random() * (max - min);
        }
    }

    public static int random(int max)
    {
        lock (randomLock)
        {
            return (int)(random() * max + 1);
        }
    }

    public static int random(int min, int max)
    {
        lock (randomLock)
        {
            return min + random(max - min);
        }
    }
    #endregion

    #region Files
    //StreamReader type should map to a Elan type FileReader
    public static StreamReader openRead(string filePath) => new StreamReader(filePath);
    public static string readLine(StreamReader file) => file.ReadLine();
    public static string readToEnd(StreamReader file) => file.ReadToEnd();
    public static void close(StreamReader file) => file.Close();
    public static bool endOfFile(StreamReader file) => file.EndOfStream;

    //StreamWriter type should map to a Elan type FilerWriter
    public static StreamWriter openWrite(string filePath) => new StreamWriter(filePath);
    public static void writeLine(StreamWriter file, string data) => file.WriteLine(data);
    public static void close(StreamWriter file) {
        file.Flush();
        file.Close();
    }

    //Separate method for binary file handling?
    #endregion

    #region Clock
    public static DateTime now() => DateTime.Now;

    public static DateOnly today() => DateOnly.FromDateTime(DateTime.Today);

    public static void pause(int milliseconds)
    {
        Thread.Sleep(milliseconds);
    }
    #endregion

    #region Guid
    public static string GUID() => Guid.NewGuid().ToString();   
    #endregion
}