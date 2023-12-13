// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
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
        var pValues = properties.Select(p => p.GetValue(o)!);
        var pStrings = pValues.Select(asString);
        var nameValues = pNames.Zip(pStrings);
        var pnv = nameValues.Select(nv => $"{nv.First}:{nv.Second}");

        var pString = string.Join(", ", pnv);

        return $"{name} {{{pString}}}";
    }

    #region tuple

    public static T1 first<T1, T2>(ValueTuple<T1, T2> t) => t.Item1;

    public static T2 second<T1, T2>(ValueTuple<T1, T2> t) => t.Item2;
   

    #endregion

    #region arrays

    public static int length<T>(ElanArray<T> l) => l.Count;

    public static bool isEmpty<T>(ElanArray<T> l) => !l.Any();

    #endregion

    #region lists

    public static int length<T>(ElanList<T> l) => l.Count;

    public static bool isEmpty<T>(ElanList<T> l) => !l.Any();

    public static ElanArray<T> asArray<T>(ElanList<T> l) => new(l);

    public static ElanList<T> insert<T>(ElanList<T> l, int index, T item) => l.Insert(index, item);


    #endregion

    #region iter

    public static int length<T>(IEnumerable<T> l) => l.Count();

    public static bool isEmpty<T>(IEnumerable<T> l) => !l.Any();

    public static ElanArray<T> asArray<T>(IEnumerable<T> l) => new(l);

    public static ElanList<T> asList<T>(IEnumerable<T> l) => new(l.ToArray());

    public static T element<T>(IEnumerable<T> e, int index) => e.Skip(index - 1).First();
    public static T head<T>(IEnumerable<T> e) => e.First();
    public static IEnumerable<T> tail<T>(IEnumerable<T> e) => e.Skip(1);
    public static IEnumerable<T> range<T>(IEnumerable<T> e, int inclusiveFrom, int exclusiveTo) => e.Skip(inclusiveFrom).Take(exclusiveTo - inclusiveFrom);
    public static IEnumerable<T> rangeFrom<T>(IEnumerable<T> e, int inclusiveFrom) => e.Skip(inclusiveFrom);
    public static IEnumerable<T> rangeTo<T>(IEnumerable<T> e, int exclusiveTo) => e.Take(exclusiveTo);

    public static bool contains<T>(IEnumerable<T> l, T element) => l.Contains(element);

    public static IEnumerable<int> integers(int from, int toInclusive) => Enumerable.Range(from, toInclusive - from + 1);

    #endregion

    #region dictionary

    public static int length<TKey, TValue>(ElanDictionary<TKey, TValue> l) where TKey : notnull => l.Count;

    public static bool isEmpty<TKey, TValue>(ElanDictionary<TKey, TValue> l) where TKey : notnull=> !l.Any();

    public static ElanList<TKey> keys<TKey, TValue>(ElanDictionary<TKey, TValue> d) where TKey : notnull => new(d.Keys.ToArray());

    public static bool hasKey<TKey, TValue>(ElanDictionary<TKey, TValue> d, TKey key) where TKey : notnull => d.ContainsKey(key);

    public static ElanList<TValue> values<TKey, TValue>(ElanDictionary<TKey, TValue> d) where TKey : notnull => new(d.Values.ToArray());

    public static ElanDictionary<TKey, TValue> removeItem<TKey, TValue>(ElanDictionary<TKey, TValue> d, TKey key) where TKey : notnull => d.Remove(key);

    public static ElanDictionary<TKey, TValue> setItem<TKey, TValue>(ElanDictionary<TKey, TValue> d, TKey key, TValue value) where TKey : notnull => d.SetItem(key, value);

    #endregion

    #region Math

    public static double absolute(double d) => Math.Abs(d);
    public static int absolute(int d) => Math.Abs(d);
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
            not null when t.Name.StartsWith("_Default") => $"default {MapTypeNames(t.BaseType)}",
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


    private static object? GetDefaultInstance(Type t) {
        return t.GetProperty("DefaultInstance") is { } p ? p.GetValue(null) : throw new NotImplementedException();
    }

    private static string TypeAsString(Type type) =>
        type.Name switch {
            "Int32" => "Int",
            "String" => "String",
            "Double" => "Float",
            "Char" => "Char",
            "Boolean" => "Bool",
            _ when type.Name.StartsWith("ElanList") => "List",
            _ when type.Name.StartsWith("ElanDictionary") => "Dictionary",
            _ when type.Name.StartsWith("ElanArray") => "Array",
            _ when type.Name.StartsWith("IEnumerable") => "Iter",
            _ => type.Name
        };

    public static string? asString(object? obj) {
        return obj switch {
            bool b => b ? "true" : "false",
            string s => s,
            ElanException e => e.message,
            null => throw new NotImplementedException(),
            _ when obj.GetType().IsAssignableTo(typeof(ITuple)) => asString<ITuple>((ITuple)obj),
            _ when obj.GetType().GetMethod("asString") is { } mi => mi.Invoke(obj, null) as string,
            IEnumerable e => e.Cast<object>().Any() ? $"Iter {{{string.Join(',', e.Cast<object>().Select(Functions.asString))}}}" : "empty iter",
            Delegate d => $"Function ({ string.Join(",", d.Method.GetParameters().Select(p => asString(p.ParameterType)))} -> {asString(d.Method.ReturnType)})",
            Type t => TypeAsString(t),
            _ => obj.ToString()
        };
    }

    public static string asString(string s) => s;

    private static string asString<T>(ITuple t) =>
        Enumerable.Range(1, t.Length - 1).Aggregate($"({asString(t[0]!)}", (s, x) => s + $", {asString(t[x]!)}") + ")";

    #endregion

    #region higher order functions

    public static IEnumerable<T> filter<T>(IEnumerable<T> source, Func<T, bool> predicate) => source.Where(predicate);

    public static IEnumerable<TResult> map<T, TResult>(IEnumerable<T> source, Func<T, TResult> predicate) => source.Select(predicate);

    public static TAccumulate reduce<T, TAccumulate>(IEnumerable<T> source, TAccumulate seed, Func<TAccumulate, T, TAccumulate> func) => source.Aggregate(seed, func);

    public static T max<T>(IEnumerable<T> source) where T : INumber<T> => source.Max()!;

    public static T maxBy<T, U>(IEnumerable<T> source, Func<T, U> predicate) where U : INumber<U> => source.MaxBy(predicate)!;

    public static T min<T>(IEnumerable<T> source) where T : INumber<T> => source.Min()!;

    public static T minBy<T,U>(IEnumerable<T> source, Func<T, U> predicate) where U : INumber<U> => source.MinBy(predicate)!;

    public static bool any<T>(IEnumerable<T> source, Func<T, bool> func) => source.Any(func);

    public static int count<T>(IEnumerable<T> source) => source.Count();

    public static IEnumerable<T> asParallel<T>(IEnumerable<T> source) => source.AsParallel();

    public static IEnumerable<ElanGroup<K,T>> groupBy<K,T>(IEnumerable<T> source, Func<T, K> func) => 
        source.GroupBy(func).Select(g => new ElanGroup<K,T>(g.Key, (IEnumerable<T>) g)).OrderBy(g => g.key);
    #endregion
}