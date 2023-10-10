using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public interface IDictionary : IEnumerable {
    public IEnumerable<object> ObjectKeys { get; }

    public IEnumerable<object> ObjectValues { get; }
}

public class ElanDictionary<TKey, TValue> : IDictionary where TKey : notnull {
    private readonly ImmutableDictionary<TKey, TValue> wrappedDictionary;

    public ElanDictionary() => wrappedDictionary = ImmutableDictionary.Create<TKey, TValue>();

    public ElanDictionary(params KeyValuePair<TKey, TValue>[] items) => wrappedDictionary = ImmutableDictionary.CreateRange(items);

    public ElanDictionary(KeyValuePair<TKey, TValue> item) => wrappedDictionary = ImmutableDictionary.CreateRange(new[] { item });

    private ElanDictionary(ImmutableDictionary<TKey, TValue> dictionary) => wrappedDictionary = dictionary;
    public IEnumerator GetEnumerator() => wrappedDictionary.GetEnumerator();


    public IEnumerable<TKey> Keys => wrappedDictionary.Keys;

    public IEnumerable<TValue> Values => wrappedDictionary.Values;

    public int Count => wrappedDictionary.Count;

    public TValue this[TKey index] => wrappedDictionary[index];
    public IEnumerable<object> ObjectKeys => Keys.Cast<object>();
    public IEnumerable<object> ObjectValues => Values.Cast<object>();
}