using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public interface IList { }

public class List<T> : IList, IEnumerable<T> {
    private readonly ImmutableList<T> wrappedList;

    public List() => wrappedList = ImmutableList.Create<T>();

    public List(params T[] items) => wrappedList = ImmutableList.Create(items);

    public List(T item) => wrappedList = ImmutableList.Create(item);

    private List(ImmutableList<T> list) => wrappedList = list;

    public int Count => wrappedList.Count;

    public T this[int index] => wrappedList[index];

    public List<T> this[Range range] => Wrap(wrappedList.ToImmutableArray()[range].ToImmutableList());

    public IEnumerator<T> GetEnumerator() => wrappedList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

    private static List<T> Wrap(ImmutableList<T> list) => new(list);

    public static List<T> operator +(List<T> a, T b) => a.Add(b);

    public static List<T> operator +(List<T> a, List<T> b) => a.AddRange(b);

    public static List<T> operator +(T a, List<T> b) => b.Insert(0, a);
}