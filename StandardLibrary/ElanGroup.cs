public class ElanGroup<K, T>
{
    public ElanGroup(K key, IEnumerable<T> members)
    {
        this.key = key;
        this.members = members;
    }

    public K key { get; init; }

    public IEnumerable<T> members { get; init; }

    public string asString() => $"Group key:{key} count: {members.Count()}";

} 