using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public class List<T> : IEnumerable<T>  {
    private readonly ImmutableList<T> wrappedList;

    public List() => wrappedList = ImmutableList.Create<T>();

    public List(params T[] items) =>  wrappedList = ImmutableList.Create<T>(items);

    public List(T item) =>  wrappedList = ImmutableList.Create<T>(item);

    private List(ImmutableList<T> list) => wrappedList = list;

    public IEnumerator<T> GetEnumerator() => wrappedList.GetEnumerator();

    public int Count => wrappedList.Count;

    public T this[int index] => wrappedList[index];

    public List<T> Add(T value) => Wrap(wrappedList.Add(value));

    public List<T> AddRange(IEnumerable<T> items) => Wrap(wrappedList.AddRange(items));

    public List<T> Clear() => Wrap(wrappedList.Clear());

    public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) => wrappedList.IndexOf(item, index, count, equalityComparer);

    public List<T> Insert(int index, T element) => Wrap(wrappedList.Insert(index, element));

    public List<T> InsertRange(int index, IEnumerable<T> items) => Wrap(wrappedList.InsertRange(index, items));

    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) => wrappedList.LastIndexOf(item, index, count, equalityComparer);

    public List<T> Remove(T value, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.Remove(value, equalityComparer));

    public List<T> RemoveAll(Predicate<T> match) => Wrap(wrappedList.RemoveAll(match));

    public List<T> RemoveAt(int index) => Wrap(wrappedList.RemoveAt(index));

    public List<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.RemoveRange(items, equalityComparer));

    public List<T> RemoveRange(int index, int count) => Wrap(wrappedList.RemoveRange(index, count));

    public List<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.Replace(oldValue, newValue, equalityComparer));

    public List<T> SetItem(int index, T value) => Wrap(wrappedList.SetItem(index, value));

    private static List<T> Wrap(ImmutableList<T> list) => new List<T>(list);

    public static List<T> operator +(List<T> a, T b) => a.Add(b);

    public static List<T> operator +(List<T> a, List<T> b) => a.AddRange(b);

    public static List<T> operator +(T a, List<T> b) => b.Insert(0, a);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}