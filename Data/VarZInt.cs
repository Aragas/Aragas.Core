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


        public byte[] Encode() => Encode(_value);


        public static VarZInt Parse(string str) => int.Parse(str);

        public static byte[] Encode(VarZInt value) => VarInt.Encode(ZigZagEncode(value._value));

        public new static VarZInt Decode(byte[] buffer, int offset) => ZigZagDecode(VarInt.Decode(buffer, offset));
        public new static VarZInt Decode(Stream stream) => ZigZagDecode(VarInt.Decode(stream));
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


        public static implicit operator VarZInt(short value) => new VarZInt(value);
        public static implicit operator short(VarZInt value) => (short) value._value;

        public static implicit operator VarZInt(int value) => new VarZInt(value);
        public static implicit operator int(VarZInt value) => value._value;

        public static implicit operator VarZInt(long value) => new VarZInt((int) value);
        public static implicit operator long(VarZInt value) => value._value;
    }
}
