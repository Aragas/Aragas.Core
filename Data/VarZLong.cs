﻿using System.IO;

namespace Aragas.Core.Data
{
    /// <summary>
    /// Encoded Int64. Optimal for negative values. Using zig-zag encoding. 
    /// </summary>
    public class VarZLong : Variant
    {
        public int Size => VariantSize((ulong) ZigZagEncode(_value));


        private readonly long _value;


        public VarZLong(long value) { _value = value; }


        public byte[] Encode() => Encode(_value);


        public static VarZLong Parse(string str) => long.Parse(str);

        public static byte[] Encode(VarZLong value) => VarLong.Encode(ZigZagEncode(value));

        public new static VarZLong Decode(byte[] buffer, int offset) => ZigZagDecode(VarLong.Decode(buffer, offset));
        public new static VarZLong Decode(Stream stream) => ZigZagDecode(VarLong.Decode(stream));
        public static int Decode(byte[] buffer, int offset, out VarZLong result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarZLong result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static implicit operator VarZLong(short value) => new VarZLong(value);
        public static implicit operator short(VarZLong value) => (short) value._value;

        public static implicit operator VarZLong(int value) => new VarZLong(value);
        public static implicit operator int(VarZLong value) => (int) value._value;

        public static implicit operator VarZLong(long value) => new VarZLong((int) value);
        public static implicit operator long(VarZLong value) => value._value;
    }
}