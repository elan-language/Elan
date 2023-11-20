// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Text;
using static StandardLibrary.Functions;

namespace StandardLibrary;

[ElanStandardLibrary]
public static class Procedures {
    #region Clock
    
    public static void pause(int milliseconds)
    {
        Thread.Sleep(milliseconds);
    }

    #endregion


    #region TODO - simple sounds

    #endregion
}