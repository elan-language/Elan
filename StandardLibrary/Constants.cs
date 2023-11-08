namespace StandardLibrary;

[ElanStandardLibrary]
public static class Constants {
    public const double pi = 3.141592653589793;

    public static string newline => @"
";

    public enum Direction
    {
        left,
        right,
        up,
        down
    }

    public enum Colour
    {
        black = 0,
        darkBlue = 1,
        darkGreen = 2,
        darkCyan = 3,
        darkRed = 4,
        darkMagenta = 5,
        darkYellow = 6,
        grey = 7,
        darkGrey = 8,
        blue = 9,
        green = 10,
        cyan = 11,
        red = 12,
        magenta = 13,
        yellow = 14,
        white = 15
    }
}