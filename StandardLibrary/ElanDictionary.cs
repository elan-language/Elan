using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public interface IElanDictionary : IEnumerable {
    public IEnumerable<object> ObjectKeys { get; }

    public IEnumerable<object> ObjectValues { get; }
}

public class ElanDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IElanDictionary where TKey : notnull {
    private readonly ImmutableDictionary<TKey, TValue> wrappedDictionary;

    public ElanDictionary() => wrappedDictionary = ImmutableDictionary.Create<TKey, TValue>();

    private ElanDictionary(params KeyValuePair<TKey, TValue>[] items) => wrappedDictionary = ImmutableDictionary.CreateRange(items);

    public ElanDictionary(params (TKey, TValue)[] items) => wrappedDictionary = ImmutableDictionary.CreateRange(items.Select(t => KeyValuePair.Create(t.Item1, t.Item2)));

    public ElanDictionary(KeyValuePair<TKey, TValue> item) => wrappedDictionary = ImmutableDictionary.CreateRange(new[] { item });

    private ElanDictionary(ImmutableDictionary<TKey, TValue> dictionary) => wrappedDictionary = dictionary;

    public static ElanDictionary<TKey, TValue> DefaultInstance { get; } = new();
    public int Count => wrappedDictionary.Count;

    public TValue this[TKey key] => wrappedDictionary[key];

    public IEnumerable<TKey> Keys => wrappedDictionary.Keys;
    public IEnumerable<TValue> Values => wrappedDictionary.Values;
    public IEnumerable<object> ObjectKeys => Keys.Cast<object>();
    public IEnumerable<object> ObjectValues => Values.Cast<object>();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => wrappedDictionary.GetEnumerator();

    public IEnumerator GetEnumerator() => wrappedDictionary.GetEnumerator();
    public bool ContainsKey(TKey key) => wrappedDictionary.ContainsKey(key);

    public bool TryGetValue(TKey key, out TValue value) => wrappedDictionary.TryGetValue(key, out value!);

    public ElanDictionary<TKey, TValue> Add(TKey key, TValue value) =>
        Wrap(wrappedDictionary.Add(key, value));

    public ElanDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs) =>
        Wrap(wrappedDictionary.AddRange(pairs));

    public ElanDictionary<TKey, TValue> Clear() =>
        Wrap(wrappedDictionary.Clear());

    public bool Contains(KeyValuePair<TKey, TValue> pair) => wrappedDictionary.Contains(pair);

    public ElanDictionary<TKey, TValue> Remove(TKey key) =>
        Wrap(wrappedDictionary.Remove(key));

    public ElanDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys) =>
        Wrap(wrappedDictionary.RemoveRange(keys));

    public ElanDictionary<TKey, TValue> SetItem(TKey key, TValue value) =>
        Wrap(wrappedDictionary.SetItem(key, value));

    public ElanDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items) =>
        Wrap(wrappedDictionary.SetItems(items));

    public bool TryGetKey(TKey equalKey, out TKey actualKey) => wrappedDictionary.TryGetKey(equalKey, out actualKey);

    private static ElanDictionary<TKey, TValue> Wrap(ImmutableDictionary<TKey, TValue> dict) => new(dict);

    public static bool operator ==(ElanDictionary<TKey, TValue> a, object? b) {
        static bool EqualKvp(KeyValuePair<TKey, TValue> kvp1, KeyValuePair<TKey, TValue> kvp2) => kvp1.Key.Equals(kvp2.Key) && kvp1.Value?.Equals(kvp2.Value) == true;

        if (b is null || b.GetType() != typeof(ElanDictionary<TKey, TValue>)) {
            return false;
        }

        var other = (ElanDictionary<TKey, TValue>)b;

        if (other.Count != a.Count) {
            return false;
        }

        return a.Zip(other).All(z => EqualKvp(z.First, z.Second));
    }

    public static bool operator !=(ElanDictionary<TKey, TValue> a, object? b) => !(a == b);

    public override bool Equals(object? obj) => this == obj;

    public override int GetHashCode() {
        static int KvpHashCode(KeyValuePair<TKey, TValue> kvp1) => kvp1.Key.GetHashCode() + (kvp1.Value?.GetHashCode() ?? 0);

        return GetType().GetHashCode() + this.Aggregate(0, (s, i) => s + KvpHashCode(i));
    }

    public override string ToString() => Functions.asString(this);
}