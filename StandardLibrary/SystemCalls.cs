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

    public static double random() => new Random().NextDouble();

    public static double random(double max) => random() * max;

    public static double random(double min, double max) => random() * (max - min) + min;

    public static int random(int max) => (int)random((double)max + 1);

    public static int random(int min, int max) => min + random(max - min);

    #endregion

    #region Files
    //var file = openRead("filename") 
    //var line = file.readLine() 
    //var file = openWrite("filename") 
    //file.writeLine

    //Separate method for binary file handling?
    #endregion

    #region Clock
    //today() 
    //now()
    //pause(100) 
    #endregion

    #region Guid
    //public static string GUID()
    #endregion
}