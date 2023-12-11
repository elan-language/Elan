// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Text;

namespace StandardLibrary;

[ElanSystemAccessor]
public static class SystemAccessors {
    #region Clock

    //public static DateTime now() => DateTime.Now;

    //public static DateOnly today() => DateOnly.FromDateTime(DateTime.Today);

    #endregion

    #region input

    public static char readKey(bool writeKey = false) {
        char k;
        if (writeKey) {
            k = Console.ReadKey().KeyChar;
        }
        else {
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
    public static void clearKeyBuffer() {
        while (Console.KeyAvailable) {
            readKey();
        }
    }

    private class NoEcho : TextWriter {
        public override Encoding Encoding => Encoding.Unicode;

        public override void Write(char value) { }
    }

    #endregion

    #region random numbers

    private static readonly object randomLock = new();

    private static Random generator = new();

    internal static void seedRandom(int seed) {
        lock (randomLock) {
            generator = new Random(seed);
        }
    }

    internal static void resetRandom() {
        lock (randomLock) {
            generator = new Random();
        }
    }

    public static double random() {
        lock (randomLock) {
            return generator.NextDouble();
        }
    }

    public static double random(double max) {
        lock (randomLock) {
            return random() * max;
        }
    }

    public static double random(double min, double max) {
        lock (randomLock) {
            return min + random() * (max - min);
        }
    }

    public static int random(int max) {
        lock (randomLock) {
            return (int)(random() * max + 1);
        }
    }

    public static int random(int min, int max) {
        lock (randomLock) {
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

    ////StreamWriter type should map to a Elan type FileWriter
    //public static StreamWriter openWrite(string filePath) => new StreamWriter(filePath);

    //Separate methods for binary file handling?

    #endregion

    #region User
    public static string me() => Environment.UserName;
    #endregion
}