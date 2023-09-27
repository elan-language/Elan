using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public class List<T> : IImmutableList<T> {
    private readonly ImmutableList<T> wrappedList;

    public List() => wrappedList = ImmutableList.Create<T>();

    public List(params T[] items) =>  wrappedList = ImmutableList.Create<T>(items);

    public List(T item) =>  wrappedList = ImmutableList.Create<T>(item);

    private List(ImmutableList<T> list) => wrappedList = list;

    public IEnumerator<T> GetEnumerator() => wrappedList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => wrappedList.Count;

    public T this[int index] => wrappedList[index];

    public IImmutableList<T> Add(T value) => Wrap(wrappedList.Add(value));

    public IImmutableList<T> AddRange(IEnumerable<T> items) => Wrap(wrappedList.AddRange(items));

    public IImmutableList<T> Clear() => Wrap(wrappedList.Clear());

    public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) => wrappedList.IndexOf(item, index, count, equalityComparer);

    public IImmutableList<T> Insert(int index, T element) => Wrap(wrappedList.Insert(index, element));

    public IImmutableList<T> InsertRange(int index, IEnumerable<T> items) => Wrap(wrappedList.InsertRange(index, items));

    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) => wrappedList.LastIndexOf(item, index, count, equalityComparer);

    public IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.Remove(value, equalityComparer));

    public IImmutableList<T> RemoveAll(Predicate<T> match) => Wrap(wrappedList.RemoveAll(match));

    public IImmutableList<T> RemoveAt(int index) => Wrap(wrappedList.RemoveAt(index));

    public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.RemoveRange(items, equalityComparer));

    public IImmutableList<T> RemoveRange(int index, int count) => Wrap(wrappedList.RemoveRange(index, count));

    public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.Replace(oldValue, newValue, equalityComparer));

    public IImmutableList<T> SetItem(int index, T value) => Wrap(wrappedList.SetItem(index, value));

    private static IImmutableList<T> Wrap(ImmutableList<T> list) => new List<T>(list);

    public static IImmutableList<T> operator +(List<T> a, T b) => a.Add(b);
}