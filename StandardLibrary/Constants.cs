using System.Collections;

namespace StandardLibrary;

[ElanStandardLibrary]
public static class Constants {
    public const double pi = 3.141592653589793;

    public static string newline = @"
";

    public class _DefaultIter<T> : IEnumerable<T> {
        public static IEnumerable<T> DefaultInstance { get; } = new _DefaultIter<T>();
        public IEnumerator<T> GetEnumerator() => new List<T>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public string asString() => "empty iter";

        public override string ToString() => asString();
    }
}