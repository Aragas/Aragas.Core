using System;
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


        public byte[] Encode() => Encode(new VarZShort(_value));


        public static VarZShort Parse(string str) => new VarZShort(short.Parse(str));

        public static byte[] Encode(VarZShort value) => VarShort.Encode(new VarShort((short) ZigZagEncode(value)));

        public new static VarZShort Decode(byte[] buffer, int offset) => new VarZShort((short) ZigZagDecode(VarShort.Decode(buffer, offset)));
        public new static VarZShort Decode(Stream stream) => new VarZShort((short) ZigZagDecode(VarShort.Decode(stream)));
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


        public static explicit operator VarZShort(short value) => new VarZShort(value);

        public static implicit operator short(VarZShort value) => value._value;
        public static implicit operator int(VarZShort value) => value._value;
        public static implicit operator long(VarZShort value) => value._value;
        public static implicit operator VarZShort(Enum value) => new VarZShort(Convert.ToInt16(value));
    }
}
