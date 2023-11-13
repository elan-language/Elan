// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Text;
using static StandardLibrary.Functions;

namespace StandardLibrary;

public static class Procedures {
    #region Clock
    public static void pause(int milliseconds) {
        Thread.Sleep(milliseconds);
    }

    #endregion

    #region Output to screen
    public static void print(object obj) => Console.WriteLine(asString(obj));

    public static void print() => Console.WriteLine();

    #endregion


    #region TODO - simple sounds

    #endregion
}