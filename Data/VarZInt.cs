using System;
using System.IO;

namespace Aragas.Core.Data
{
    /// <summary>
    /// Encoded Int32. Optimal for negative values. Using zig-zag encoding. 
    /// </summary>
    public class VarZInt : Variant
    {
        public int Size => VariantSize((uint) ZigZagEncode(_value));


        private readonly int _value;


        public VarZInt(int value) { _value = value; }


        public byte[] Encode() => Encode(new VarZInt(_value));


        public static VarZInt Parse(string str) => new VarZInt(int.Parse(str));

        public static byte[] Encode(VarZInt value) => VarInt.Encode(new VarInt((int) ZigZagEncode(value._value)));

        public new static VarZInt Decode(byte[] buffer, int offset) => new VarZInt((int) ZigZagDecode(VarInt.Decode(buffer, offset)));
        public new static VarZInt Decode(Stream stream) => new VarZInt((int) ZigZagDecode(VarInt.Decode(stream)));
        public static int Decode(byte[] buffer, int offset, out VarZInt result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarZInt result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static explicit operator VarZInt(short value) => new VarZInt(value);
        public static explicit operator VarZInt(int value) => new VarZInt(value);

        public static implicit operator short(VarZInt value) => (short) value._value;
        public static implicit operator int(VarZInt value) => value._value;
        public static implicit operator long(VarZInt value) => value._value;
        public static implicit operator VarZInt(Enum value) => new VarZInt(Convert.ToInt32(value));
    }
}
