using System.Collections;

namespace StandardLibrary;

public class ElanString : ElanIter<char> {
    private readonly string wrappedString;

    public ElanString(string wrappedString) => this.wrappedString = wrappedString;
    public IEnumerator<char> GetEnumerator() => wrappedString.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public char this[int index] => wrappedString[index];

    public ElanIter<char> this[Range range] => new ElanString(wrappedString[range]);

    public static implicit operator string(ElanString es) => es.wrappedString;

    public static implicit operator ElanString(string s) => new(s);
}