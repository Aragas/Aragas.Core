using System.IO;

namespace Aragas.Core.Data
{
    /// <summary>
    /// Encoded Int16. Optimal for negative values. Using zig-zag encoding.
    /// </summary>
    public class VarZShort : Variant
    {
        public int Size => VariantSize((ushort) ZigZagEncode(_value));


        private readonly short _value;


        public VarZShort(short value) { _value = value; }


        public byte[] Encode() => Encode(_value);


        public static VarZShort Parse(string str) => short.Parse(str);

        public static byte[] Encode(VarZShort value) => VarShort.Encode(ZigZagEncode(value));

        public new static VarZShort Decode(byte[] buffer, int offset) => ZigZagDecode(VarShort.Decode(buffer, offset));
        public new static VarZShort Decode(Stream stream) => ZigZagDecode(VarShort.Decode(stream));
        public static int Decode(byte[] buffer, int offset, out VarZShort result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarZShort result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static implicit operator VarZShort(short value) => new VarZShort(value);
        public static implicit operator short(VarZShort value) => value._value;

        public static implicit operator VarZShort(int value) => new VarZShort((short) value);
        public static implicit operator int(VarZShort value) => value._value;

        public static implicit operator VarZShort(long value) => new VarZShort((short) value);
        public static implicit operator long(VarZShort value) => value._value;
    }
}
