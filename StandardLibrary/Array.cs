using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public interface IArray { }

public class Array<T> : IArray, IEnumerable<T> {
    private readonly T[] wrappedArray;

    public Array() => wrappedArray = wrappedArray = Array.Empty<T>();
    
    public Array(int size) {
        wrappedArray = new T[size];

        Array.Fill(wrappedArray, Default);
    }

    private static T? Default => typeof(T) == typeof(string) ? (T)(object)"" : default;

    public int Count => wrappedArray.Length;

    public IEnumerator<T> GetEnumerator() => wrappedArray.ToList().GetEnumerator();

    public T this[int index] => wrappedArray[index];

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}