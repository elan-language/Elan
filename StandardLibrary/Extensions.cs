namespace StandardLibrary; 

public static class Extensions {
    public static void Deconstruct<T>(this IEnumerable<T> source, out T head, out IEnumerable<T> tail) {
        head = source.First();
        tail = source.Skip(1);
    }
}