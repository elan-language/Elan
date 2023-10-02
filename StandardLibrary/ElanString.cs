using System.Collections;
using System.Text;

namespace StandardLibrary;

// implements all interfaces that string does through the field content
public sealed class ElanString : IEnumerable<char>, ICloneable, IComparable, IComparable<string>, IConvertible, IEquatable<string> {
    private readonly string content;

    private ElanString(string content) => this.content = content;

    // implicit conversions
    public static implicit operator string(ElanString d) => d.content;
    public static implicit operator ElanString(string b) => new(b);

    public static bool operator <(ElanString lhs, ElanString rhs) => lhs.content.CompareTo(rhs.content) < 0;

    public static bool operator >(ElanString lhs, ElanString rhs) => lhs.content.CompareTo(rhs.content) > 0;

    // string supports it, why shouldnt we?
    public static ElanString operator +(ElanString lhs, ElanString rhs) {
        var sb = new StringBuilder();
        sb.Append(lhs.content);
        sb.Append(rhs.content);
        return sb.ToString();
    }

    // at request of @Alexey Khoroshikh
    public static ElanString operator *(ElanString lhs, int rhs) {
        var sb = new StringBuilder();
        for (var i = 0; i < rhs; i++) {
            sb.Append(lhs.content);
        }

        return sb.ToString();
    }

    // other nice thing to have
    public static string[] operator /(ElanString lhs, char rhs) => lhs.content.Split(rhs);

    public override bool Equals(object obj) =>
        (obj is ElanString wrapper && content == wrapper.content)
        || (obj is string str && content == str);

    #region auto-generated code through visual studio

    public override int GetHashCode() => -1896430574 + EqualityComparer<string>.Default.GetHashCode(content);

    public override string ToString() => content;

    public object Clone() => content.Clone();

    public int CompareTo(string? other) => content.CompareTo(other);

    public bool Equals(string? other) => content.Equals(other);

    public IEnumerator<char> GetEnumerator() => ((IEnumerable<char>)content).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)content).GetEnumerator();

    public int CompareTo(object? obj) => content.CompareTo(obj);

    public TypeCode GetTypeCode() => content.GetTypeCode();

    public bool ToBoolean(IFormatProvider? provider) => ((IConvertible)content).ToBoolean(provider);

    public byte ToByte(IFormatProvider? provider) => ((IConvertible)content).ToByte(provider);

    public char ToChar(IFormatProvider? provider) => ((IConvertible)content).ToChar(provider);

    public DateTime ToDateTime(IFormatProvider? provider) => ((IConvertible)content).ToDateTime(provider);

    public decimal ToDecimal(IFormatProvider? provider) => ((IConvertible)content).ToDecimal(provider);

    public double ToDouble(IFormatProvider? provider) => ((IConvertible)content).ToDouble(provider);

    public short ToInt16(IFormatProvider? provider) => ((IConvertible)content).ToInt16(provider);

    public int ToInt32(IFormatProvider? provider) => ((IConvertible)content).ToInt32(provider);

    public long ToInt64(IFormatProvider? provider) => ((IConvertible)content).ToInt64(provider);

    public sbyte ToSByte(IFormatProvider? provider) => ((IConvertible)content).ToSByte(provider);

    public float ToSingle(IFormatProvider? provider) => ((IConvertible)content).ToSingle(provider);

    public string ToString(IFormatProvider? provider) => content.ToString(provider);

    public object ToType(Type conversionType, IFormatProvider? provider) => ((IConvertible)content).ToType(conversionType, provider);

    public ushort ToUInt16(IFormatProvider? provider) => ((IConvertible)content).ToUInt16(provider);

    public uint ToUInt32(IFormatProvider? provider) => ((IConvertible)content).ToUInt32(provider);

    public ulong ToUInt64(IFormatProvider? provider) => ((IConvertible)content).ToUInt64(provider);

    #endregion auto-generated code through visual studio
}