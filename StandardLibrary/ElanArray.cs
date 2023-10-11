using System.Collections;

namespace StandardLibrary;

public interface IArray { }

public class ElanArray<T> : IArray, IEnumerable<T> {
    private readonly bool twoD;
    private readonly T[][] wrappedArray;

    public ElanArray() : this(0) { }

    public ElanArray(IEnumerable<T> init) {
        wrappedArray = new T[1][];
        wrappedArray[0] = init.ToArray();
    }

    public ElanArray(int size) {
        wrappedArray = new T[1][];
        wrappedArray[0] = new T[size];

        Array.Fill(wrappedArray[0], Default);
    }

    public ElanArray(int size1, int size2) {
        twoD = true;
        wrappedArray = new T[size1][];

        for (var i = 0; i < size1; i++) {
            wrappedArray[i] = new T[size2];
            Array.Fill(wrappedArray[i], Default);
        }
    }

    private static T? Default => typeof(T) == typeof(string) ? (T)(object)"" : default;

    public int Count => wrappedArray.Length * wrappedArray[0].Length;

    public T this[int index] {
        get => wrappedArray[0][index];
        set => wrappedArray[0][index] = value;
    }

    public T this[int index1, int index2] {
        get => twoD ? wrappedArray[index1][index2] : throw new IndexOutOfRangeException();
        set {
            if (twoD) {
                wrappedArray[index1][index2] = value;
            }
            else {
                throw new IndexOutOfRangeException();
            }
        }
    }

    public IEnumerator<T> GetEnumerator() {
        foreach (var arr in wrappedArray) {
            foreach (var item in arr) {
                yield return item;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(T item) {
        wrappedArray[0] = wrappedArray[0].Append(item).ToArray();
    }
}