﻿using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public interface IElanList { }

public class ElanList<T> : IElanList, IEnumerable<T> {
    private readonly ImmutableList<T> wrappedList;

    public ElanList() => wrappedList = ImmutableList.Create<T>();

    public ElanList(params T[] items) => wrappedList = ImmutableList.Create(items);

    public ElanList(T item) => wrappedList = ImmutableList.Create(item);

    private ElanList(ImmutableList<T> list) => wrappedList = list;

    public int Count => wrappedList.Count;

    public T this[int index] => wrappedList[index];

    public ElanList<T> this[Range range] => Wrap(wrappedList.ToImmutableArray()[range].ToImmutableList());

    public IEnumerator<T> GetEnumerator() => wrappedList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public ElanList<T> Add(T value) => Wrap(wrappedList.Add(value));

    public ElanList<T> AddRange(IEnumerable<T> items) => Wrap(wrappedList.AddRange(items));

    public ElanList<T> Clear() => Wrap(wrappedList.Clear());

    public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) => wrappedList.IndexOf(item, index, count, equalityComparer);

    public ElanList<T> Insert(int index, T element) => Wrap(wrappedList.Insert(index, element));

    public ElanList<T> InsertRange(int index, IEnumerable<T> items) => Wrap(wrappedList.InsertRange(index, items));

    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) => wrappedList.LastIndexOf(item, index, count, equalityComparer);

    public ElanList<T> Remove(T value, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.Remove(value, equalityComparer));

    public ElanList<T> RemoveAll(Predicate<T> match) => Wrap(wrappedList.RemoveAll(match));

    public ElanList<T> RemoveAt(int index) => Wrap(wrappedList.RemoveAt(index));

    public ElanList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.RemoveRange(items, equalityComparer));

    public ElanList<T> RemoveRange(int index, int count) => Wrap(wrappedList.RemoveRange(index, count));

    public ElanList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) => Wrap(wrappedList.Replace(oldValue, newValue, equalityComparer));

    public ElanList<T> SetItem(int index, T value) => Wrap(wrappedList.SetItem(index, value));

    private static ElanList<T> Wrap(ImmutableList<T> list) => new(list);

    public static ElanList<T> operator +(ElanList<T> a, T b) => a.Add(b);

    public static ElanList<T> operator +(ElanList<T> a, ElanList<T> b) => a.AddRange(b);

    public static ElanList<T> operator +(T a, ElanList<T> b) => b.Insert(0, a);
}