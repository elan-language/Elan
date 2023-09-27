// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StandardLibrary;

[ElanStandardLibrary]
public static class Functions {
    #region Math

    public const double pi = 3.141592653589793;
    public static double aCos(double d) => Math.Cos(d);
    public static double aCosh(double d) => Math.Acosh(d);
    public static double aSin(double d) => Math.Asin(d);
    public static double aSinh(double d) => Math.Asinh(d);
    public static double aTan(double d) => Math.Atan(d);
    public static double aTanh(double d) => Math.Atanh(d);
    public static int roundUp(double d) => (int)Math.Ceiling(d);
    public static double cos(double d) => Math.Cos(d);
    public static double cosh(double d) => Math.Cosh(d);
    public static double exp(double d) => Math.Exp(d);
    public static int roundDown(double d) => (int)Math.Floor(d);
    public static double log(double d) => Math.Log(d);
    public static double log2(double x) => Math.Log2(x);
    public static double log10(double d) => Math.Log10(d);
    public static double sin(double d) => Math.Sin(d);
    public static double sinh(double d) => Math.Sinh(d);
    public static double sqrt(double d) => Math.Sqrt(d);
    public static double tan(double d) => Math.Tan(d);
    public static double tanh(double d) => Math.Tanh(d);

    public static double min(double a, double b) => Math.Min(a, b);
    public static double max(double a, double b) => Math.Max(a, b);

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

    // TYpe is not a yet an Elan thing 
    //public static Type GetType(object o) => o.GetType();
    //public static bool ImplementsType<T>(object o) => o.GetType() == typeof(T) ||  o.GetType().IsSubclassOf(typeof(T));

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


    #endregion

    #region Bitwise operations
    public static int bitwiseNot(int x) => ~x;
    public static int bitwiseAnd(int x, int y) => x & y;
    public static int bitwiseOr(int x, int y) => x | y;
    public static int bitwiseXor(int x, int y) => x ^ y;
    public static int shiftLeft(int x, int places) => x << places;
    public static int shiftRight(int x, int places) => x >> places;
    #endregion

    #region AsString

    public static string? asString(object obj) {
      
        return obj switch {
            bool b => b ? "true" : "false",
            string s => s,
            IEnumerable l => asString(l),
            _ when obj.GetType().IsAssignableTo(typeof(ITuple)) => asString<ITuple>((ITuple)obj),
            _ => obj.ToString()
        };
    }

    public static string asString(string s) => s;

    public static string asString(IEnumerable l) {
        var a = l.Cast<object>().ToArray();

        return a.Length == 0 ? "empty list" : $"List {{{string.Join(',', a)}}}";
    }
    
    public static string asString(IList l) =>
        l.Count == 0 ? "empty list" :
            Enumerable.Range(1, l.Count - 1).Aggregate($"List {{{asString(l[0])}", (s, n) => s + $",{asString(l[n])}") + $"}}";

    public static string asString<T>(StandardLibrary.List<T> l) =>
        l.Count == 0 ? "empty List" :
        Enumerable.Range(1, l.Count - 1).Aggregate($"{{{asString(l[0])}", (s, n) => s + $",{asString(l[n])}") + $"}}";

    public static string asString<T>(ImmutableList<T> l) =>
        l.Count == 0 ? "empty list" :
            Enumerable.Range(1, l.Count - 1).Aggregate($"List {{{asString(l[0])}", (s, n) => s + $",{asString(l[n])}") + $"}}";

    public static string asString<T>(ITuple t) =>
        Enumerable.Range(1, t.Length - 1).Aggregate($"({asString(t[0])}", (s, x) => s + $", {asString(t[x])}") + $")";

    public static string asString<T>(T[] a) =>
         a.Length == 0 ? "empty Array" :
        Enumerable.Range(1, a.Length - 1).Aggregate($"Array {{{asString(a[0])}", (s, n) => s + $",{asString(a[n])}") + $"}}";

    #endregion

    #region lists

    public static int length(IEnumerable l) => l.Cast<object>().ToArray().Length;

    #endregion

    #region String handling
    public const string newLine = @"
" ;
    #endregion
}