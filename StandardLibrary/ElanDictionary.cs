using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public interface IElanDictionary : IEnumerable {
    public IEnumerable<object> ObjectKeys { get; }

    public IEnumerable<object> ObjectValues { get; }
}

public class ElanDictionary<TKey, TValue> : IImmutableDictionary<TKey, TValue>, IElanDictionary where TKey : notnull {

    public static ElanDictionary<TKey, TValue> DefaultInstance { get; } = new();

    private readonly ImmutableDictionary<TKey, TValue> wrappedDictionary;

    public ElanDictionary() => wrappedDictionary = ImmutableDictionary.Create<TKey, TValue>();

    private ElanDictionary(params KeyValuePair<TKey, TValue>[] items) => wrappedDictionary = ImmutableDictionary.CreateRange(items);

    public ElanDictionary(params (TKey, TValue)[] items) => wrappedDictionary = ImmutableDictionary.CreateRange(items.Select(t => KeyValuePair.Create(t.Item1, t.Item2)));

    public ElanDictionary(KeyValuePair<TKey, TValue> item) => wrappedDictionary = ImmutableDictionary.CreateRange(new[] { item });

    private ElanDictionary(ImmutableDictionary<TKey, TValue> dictionary) => wrappedDictionary = dictionary;
    public IEnumerable<object> ObjectKeys => Keys.Cast<object>();
    public IEnumerable<object> ObjectValues => Values.Cast<object>();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => throw new NotImplementedException();

    public IEnumerator GetEnumerator() => wrappedDictionary.GetEnumerator();
    public int Count => wrappedDictionary.Count;
    public bool ContainsKey(TKey key) => wrappedDictionary.ContainsKey(key);

    public bool TryGetValue(TKey key, out TValue value) => wrappedDictionary.TryGetValue(key, out value!);

    public TValue this[TKey key] => wrappedDictionary[key];

    public IEnumerable<TKey> Keys => wrappedDictionary.Keys;
    public IEnumerable<TValue> Values => wrappedDictionary.Values;

    public IImmutableDictionary<TKey, TValue> Add(TKey key, TValue value) =>
        Wrap(wrappedDictionary.Add(key, value));

    public IImmutableDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs) =>
        Wrap(wrappedDictionary.AddRange(pairs));

    public IImmutableDictionary<TKey, TValue> Clear() =>
        Wrap(wrappedDictionary.Clear());

    public bool Contains(KeyValuePair<TKey, TValue> pair) => wrappedDictionary.Contains(pair);

    public IImmutableDictionary<TKey, TValue> Remove(TKey key) =>
        Wrap(wrappedDictionary.Remove(key));

    public IImmutableDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys) =>
        Wrap(wrappedDictionary.RemoveRange(keys));

    public IImmutableDictionary<TKey, TValue> SetItem(TKey key, TValue value) =>
        Wrap(wrappedDictionary.SetItem(key, value));

    public IImmutableDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items) =>
        Wrap(wrappedDictionary.SetItems(items));

    public bool TryGetKey(TKey equalKey, out TKey actualKey) => wrappedDictionary.TryGetKey(equalKey, out actualKey);

    private static ElanDictionary<TKey, TValue> Wrap(ImmutableDictionary<TKey, TValue> dict) => new(dict);
}