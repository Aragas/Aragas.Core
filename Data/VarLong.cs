using System;
using System.IO;

namespace Aragas.Core.Data
{
    /// <summary>
    /// Encoded Int64. Not optimal for negative values.
    /// </summary>
    public class VarLong : Variant
    {
        public int Size => VariantSize((ulong) _value);


        private readonly long _value;


        public VarLong(long value) { _value = value; }


        public byte[] Encode() => Encode(_value);


        public override string ToString() => _value.ToString();

        public static VarLong Parse(string str) => long.Parse(str);

        public static byte[] Encode(VarLong value) => Variant.Encode((ulong) value._value);
        public static int Encode(VarLong value, byte[] buffer, int offset)
        {
            var encoded = value.Encode();
            Buffer.BlockCopy(encoded, 0, buffer, offset, encoded.Length);
            return encoded.Length;
        }
        public static int Encode(VarLong value, Stream stream)
        {
            var encoded = value.Encode();
            stream.Write(encoded, 0, encoded.Length);
            return encoded.Length;
        }

        public new static VarLong Decode(byte[] buffer, int offset) => new VarLong((long) Variant.Decode(buffer, offset));
        public new static VarLong Decode(Stream stream) => new VarLong((long) Variant.Decode(stream));
        public static int Decode(byte[] buffer, int offset, out VarLong result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarLong result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static implicit operator VarLong(short value) => new VarLong(value);
        public static implicit operator short(VarLong value) => (short) value._value;

        public static implicit operator VarLong(int value) => new VarLong(value);
        public static implicit operator int(VarLong value) => (int) value._value;

        public static implicit operator VarLong(long value) => new VarLong((int) value);
        public static implicit operator long(VarLong value) => value._value;
    }
}
