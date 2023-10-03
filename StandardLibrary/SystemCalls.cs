// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
using System.ComponentModel;
using System.Text;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

namespace StandardLibrary;

[ElanSystemCall]
public static class SystemCalls
{
    #region Output to screen

    //TODO once we can support Any type - replace all overloads with argument of type 'object'
    public static void printLine(object obj) => Console.WriteLine(asString(obj));

    public static void printLine() => Console.WriteLine();

    public static void print(object obj) => Console.Write(asString(obj));
    #endregion

    #region Input from keyboard
    public static string input(string prompt)
    {
        Console.Write(prompt);
        return input();
    }

    public static string input()
    {
        return Console.ReadLine();
    }

    public static char readKey(bool writeKey = false)
    {
        char k;
        if (writeKey)
        {
            k = Console.ReadKey().KeyChar;
        }
        else
        {
            Console.CursorVisible = false;
            var standardOut = Console.Out;
            Console.SetOut(new NoEcho());
            k = Console.ReadKey().KeyChar;
            Console.SetOut(standardOut);
            Console.CursorVisible = true;
        }
        return k;
    }

    public static bool keyHasBeenPressed() => Console.KeyAvailable;

    //Ensures that there are no previously hit keys waiting to be read
    public static void clearKeyBuffer()
    {
        while (Console.KeyAvailable)
            readKey();
    }

    private class NoEcho : TextWriter
    {
        public override void Write(char value) { }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
    #endregion

    #region TODO - simple sounds

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

    public static void resetRandom()
    {
        lock (randomLock)
        {
            generator = new Random();
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

    //public static T random<T>(IEnumerable<T> source)
    //{
    //    lock (randomLock)
    //    {
    //        return source.ElementAt(random(source.Count() -1));
    //    }
    //}
    #endregion

    #region Files
    ////StreamReader type should map to a Elan type FileReader
    //public static StreamReader openRead(string filePath) => new StreamReader(filePath);
    //public static string readLine(StreamReader file) => file.ReadLine();
    //public static string readToEnd(StreamReader file) => file.ReadToEnd();
    //public static void close(StreamReader file) => file.Close();
    //public static bool endOfFile(StreamReader file) => file.EndOfStream;

    ////StreamWriter type should map to a Elan type FilerWriter
    //public static StreamWriter openWrite(string filePath) => new StreamWriter(filePath);
    //public static void writeLine(StreamWriter file, string data) => file.WriteLine(data);
    //public static void close(StreamWriter file) {
    //    file.Flush();
    //    file.Close();
    //}

    //Separate method for binary file handling?
    #endregion

    #region Clock
    //public static DateTime now() => DateTime.Now;

    //public static DateOnly today() => DateOnly.FromDateTime(DateTime.Today);

    public static void pause(int milliseconds)
    {
        Thread.Sleep(milliseconds);
    }
    #endregion

    #region Guid
    //public static string Guid() => Guid.NewGuid().ToString();   
    #endregion
}