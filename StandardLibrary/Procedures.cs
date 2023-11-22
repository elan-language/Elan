// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace StandardLibrary;

[ElanStandardLibrary]
public static class Procedures {
    #region Clock

    public static void pause(int milliseconds) {
        Thread.Sleep(milliseconds);
    }

    #endregion
    
    public static void seedRandom(int seed) {
        SystemAccessors.seedRandom(seed);
    }

    public static void resetRandom() {
        SystemAccessors.resetRandom();
    }



    #region TODO - simple sounds

    #endregion
}