using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StandardLibrary;

internal class DefaultIter<T> : ElanIter<T> {
    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T this[int index] => throw new NotImplementedException();

    public ElanIter<T> this[Range range] => throw new NotImplementedException();

    public static ElanIter<T> DefaultInstance { get; } = new DefaultIter<T>();
    public string asString() => "empty iter";

}


public interface ElanIter<T> : IEnumerable<T> {
    public T this[int index] { get; }

    public ElanIter<T> this[Range range] { get; }

    public static ElanIter<T> DefaultInstance { get; } = DefaultIter<T>.DefaultInstance;
}