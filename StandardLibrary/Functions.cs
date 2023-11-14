// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Collections;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using static StandardLibrary.Constants;

namespace StandardLibrary;

[ElanStandardLibrary]
public static class Functions {

    public static string typeAndProperties(object o) {
        var type = o.GetType();
        var name = type.Name;
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetCustomAttribute<CompilerGeneratedAttribute>() is null).ToArray();
        var pNames = properties.Select(p => p.Name);
        var pValues = properties.Select(p => p.GetValue(o));
        var pStrings = pValues.Select(asString);
        var nameValues = pNames.Zip(pStrings);
        var pnv = nameValues.Select(nv => $"{nv.First}:{nv.Second}");

        var pString = string.Join(", ", pnv);

        return $"{name} {{{pString}}}";
    }

    #region lists

    public static int length(IEnumerable l) => l.Cast<object>().ToArray().Length;

    public static bool isEmpty(IEnumerable l) => !l.Cast<object>().Any();

    public static ElanArray<T> asArray<T>(IEnumerable<T> l) => new(l);

    #endregion

    #region dictionary

    public static IEnumerable<TKey> keys<TKey, TValue>(ElanDictionary<TKey, TValue> d) where TKey : notnull => d.Keys;

    public static bool hasKey<TKey, TValue>(ElanDictionary<TKey, TValue> d, TKey key) where TKey : notnull => d.ContainsKey(key);

    public static IEnumerable<TValue> values<TKey, TValue>(ElanDictionary<TKey, TValue> d) where TKey : notnull => d.Values;

    public static ElanDictionary<TKey, TValue> removeItem<TKey, TValue>(ElanDictionary<TKey, TValue> d, TKey key) where TKey : notnull => d.Remove(key);

    public static ElanDictionary<TKey, TValue> setItem<TKey, TValue>(ElanDictionary<TKey, TValue> d, TKey key, TValue value) where TKey : notnull => d.SetItem(key, value);

    #endregion

    #region Math

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

    public static string newlines(int n) => Enumerable.Range(1, n).Aggregate("", (s, x) => s + newline);
    public static string asUpperCase(string s) => s.ToUpper();
    public static string asLowerCase(string s) => s.ToLower();

    public static bool isBefore(string a, string b) => a.CompareTo(b) < 0;
    public static bool isBeforeOrSameAs(string a, string b) => a.CompareTo(b) <= 0;
    public static bool isAfter(string a, string b) => a.CompareTo(b) > 0;
    public static bool isAfterOrSameAs(string a, string b) => a.CompareTo(b) >= 0;

    public static bool contains(string s, string x) => s.Contains(x);
    public static bool contains(string s, char x) => s.Contains(x);

    #endregion

    #region Type inspection




    private static string MapTypeNames(Type? t) {
        return t?.Name switch {
            "Int32" => "Int",
            "Double" => "Float",
            "Char" => "Char",
            "Boolean" => "Bool",
            "String" => "String",
            "ElanList`1" => $"List<{MapTypeNames(t.GenericTypeArguments.Single())}>",
            "ElanArray`1" => $"Array<{MapTypeNames(t.GenericTypeArguments.Single())}>",
            "ElanDictionary`2" => $"Dictionary<{MapTypeNames(t.GenericTypeArguments.First())},{MapTypeNames(t.GenericTypeArguments.Last())}>",
            _ => t?.Name ?? throw new NotImplementedException()
        };
    }




    public static string type(object o) => MapTypeNames(o.GetType());

    #endregion

    #region Type conversion

    public static double asFloat(int x) => x;
    public static int asInt(double x) => (int)x;
    public static char asChar(int unicode) => (char)unicode;
    public static int unicodeDecimalValue(char c) => c;
    public static string unicodeHexValue(char c) => ((int)c).ToString("X4)");
    public static int asInt(string s) => int.Parse(s);
    public static double asFloat(string s) => double.Parse(s);

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
            IElanDictionary d => asString(d),
            IEnumerable l => asString(l),
            ElanException e => e.message,
            _ when obj.GetType().IsAssignableTo(typeof(ITuple)) => asString<ITuple>((ITuple)obj),
            _ when obj.GetType().GetMethod("asString") is { } mi => mi.Invoke(obj, null) as string,
            _ => obj.ToString()
        };
    }

    public static string asString(string s) => s;

    private static string EmptyMessage(IEnumerable e) => e is IElanArray ? "empty array" : e is IElanDictionary ? "empty dictionary" : "empty list";

    private static string Type(IEnumerable e) => e is IElanArray ? "Array" : e is IElanDictionary ? "Dictionary" : "List";

    public static string asString(IEnumerable e) {
        var a = e.Cast<object>().Select(asString).ToArray();
        return a.Any() ? $"{Type(e)} {{{string.Join(',', a)}}}" : EmptyMessage(e);
    }

    public static string asString(IElanDictionary d) {
        var keys = d.ObjectKeys.Select(asString).ToArray();
        var values = d.ObjectValues.Select(asString).ToArray();
        var ss = keys.Zip(values).Select(i => $"{i.First}:{i.Second}").ToArray();

        return ss.Any() ? $"{Type(d)} {{{string.Join(',', ss)}}}" : EmptyMessage(d);
    }

    public static string asString<T>(ITuple t) =>
        Enumerable.Range(1, t.Length - 1).Aggregate($"({asString(t[0])}", (s, x) => s + $", {asString(t[x])}") + ")";

    #endregion
}