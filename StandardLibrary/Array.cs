using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public class Array<T> : IEnumerable<T> {
    private readonly T[] wrappedArray;

    public Array() => wrappedArray = wrappedArray = Array.Empty<T>();
    
    public Array(int size) => wrappedArray = new T[size];

    public int Count => wrappedArray.Length;

    public IEnumerator<T> GetEnumerator() => wrappedArray.ToList().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}