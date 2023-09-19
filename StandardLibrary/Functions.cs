// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace StandardLibrary;

public static class Functions {
    #region Math

    public static double pi => Math.PI;
    public static double aCos(double d) => Math.Cos(d);
    public static double aCosh(double d) => Math.Acosh(d);
    public static double aSin(double d) => Math.Asin(d);
    public static double aSinh(double d) => Math.Asinh(d);
    public static double aTan(double d) => Math.Atan(d);
    public static double aTanh(double d) => Math.Atanh(d);
    public static int roundUp(double d) => (int)Math.Ceiling(d);
    public static double cos(double d) => Math.Cosh(d);
    public static double cosh(double d) => Math.Cosh(d);
    public static double exp(double d) => Math.Exp(d);
    public static int roundDown(double d) => (int)Math.Floor(d);
    public static double log(double d) => Math.Log(d);
    public static double log2(double x) => Math.Log2(x);
    public static double log10(double d) => Math.Log10(d);
    public static double pow(double x, double y) => Math.Pow(x, y);
    public static double sin(double d) => Math.Sin(d);
    public static double sinh(double d) => Math.Sinh(d);
    public static double sqrt(double d) => Math.Sqrt(d);
    public static double tan(double d) => Math.Tan(d);
    public static double tanh(double d) => Math.Tanh(d);

    #endregion

    #region String handling

    public static int length(string s) => s.Length;
    public static int indexOf(string s, char c) => s.IndexOf(c);
    public static int startIndexOf(string s, string subString) => s.IndexOf(subString);

    public static string newline => @"
";

    public static string newlines(int n) => Enumerable.Range(1, n).Aggregate("", (s, x) => s + newline);
    public static string asUpperCase(string s) => s.ToUpper();
    public static string asLowerCase(string s) => s.ToLower();

    #endregion

    #region Type handling

    public static Type GetType(object o) => throw new NotImplementedException();
    public static bool ImplementsType<T>(object o) => throw new NotImplementedException();

    public static double asFloat(int x) => x;
    public static double asFloat(decimal x) => (double)x;
    public static decimal asDecimal(int x) => x;
    public static decimal asDecimal(float x) => (decimal)x;
    public static int asInt(double x) => (int)x;
    public static int asInt(decimal x) => (int)x;
    public static char asChar(int unicode) => (char)unicode;
    public static int unicodeDecimalValue(char c) => c;
    public static string unicodeHexValue(char c) => ((int)c).ToString("X4)");
    public static int asInt(string s) => int.Parse(s);
    public static double asFloat(string s) => double.Parse(s);
    public static decimal asDecimal(string s) => decimal.Parse(s);

    //TODO  asString(object a) => a.ToString();

    #endregion

    #region Bitwise operations
    public static int bitwiseNot(int x) => throw new NotImplementedException();
    public static int bitwiseAnd(int x, int y) => throw new NotImplementedException();
    public static int bitwiseOr(int x, int y) => throw new NotImplementedException();
    public static int bitwiseXor(int x, int y) => throw new NotImplementedException();
    public static int bitwiseShiftLeft(int x, int places) => throw new NotImplementedException();
    public static int bitwiseShiftRight(int x, int places) => throw new NotImplementedException();
    #endregion
}