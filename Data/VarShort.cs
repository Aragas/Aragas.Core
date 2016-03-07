using System;
using System.IO;

namespace Aragas.Core.Data
{
    /// <summary>
    /// Encoded Int16. Not optimal for negative values.
    /// </summary>
    public class VarShort : Variant
    {
        public int Size => VariantSize((ushort) _value);


        private readonly short _value;


        public VarShort(short value) { _value = value; }


        public byte[] Encode() => Encode(_value);


        public override string ToString() => _value.ToString();

        public static VarShort Parse(string str) => short.Parse(str);

        public static byte[] Encode(VarShort value) => Variant.Encode((ushort) value._value);
        public static int Encode(VarShort value, byte[] buffer, int offset)
        {
            var encoded = value.Encode();
            Buffer.BlockCopy(encoded, 0, buffer, offset, encoded.Length);
            return encoded.Length;
        }
        public static int Encode(VarShort value, Stream stream)
        {
            var encoded = value.Encode();
            stream.Write(encoded, 0, encoded.Length);
            return encoded.Length;
        }

        public new static VarShort Decode(byte[] buffer, int offset) => new VarShort((short) Variant.Decode(buffer, offset));
        public new static VarShort Decode(Stream stream) => new VarShort((short) Variant.Decode(stream));
        public static int Decode(byte[] buffer, int offset, out VarShort result)
        {
            result = (short) Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarShort result)
        {
            result = (short) Decode(stream);
            return result.Size;
        }


        public static implicit operator VarShort(short value) => new VarShort(value);
        public static implicit operator short(VarShort value) => value._value;

        public static implicit operator VarShort(int value) => new VarShort((short) value);
        public static implicit operator int(VarShort value) => value._value;

        public static implicit operator VarShort(long value) => new VarShort((short) value);
        public static implicit operator long(VarShort value) => value._value;
    }
}