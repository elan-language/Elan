using System.Collections;
using System.Collections.Immutable;

namespace StandardLibrary;

public interface IDictionary : IEnumerable {
    public IEnumerable<object> Keys { get; }

    public IEnumerable<object> Values { get; }
}

public class ElanDictionary<TKey, TValue> : IDictionary where TKey : notnull {
    private readonly ImmutableDictionary<TKey, TValue> wrappedDictionary;

    public ElanDictionary() => wrappedDictionary = ImmutableDictionary.Create<TKey, TValue>();

    public ElanDictionary(params KeyValuePair<TKey, TValue>[] items) => wrappedDictionary = ImmutableDictionary.CreateRange(items);

    public ElanDictionary(KeyValuePair<TKey, TValue> item) => wrappedDictionary = ImmutableDictionary.CreateRange(new[] { item });

    private ElanDictionary(ImmutableDictionary<TKey, TValue> dictionary) => wrappedDictionary = dictionary;
    public IEnumerator GetEnumerator() => wrappedDictionary.GetEnumerator();

    public IEnumerable<object> Keys => wrappedDictionary.Keys.Cast<object>();

    public IEnumerable<object> Values => wrappedDictionary.Values.Cast<object>();
}