using System;
using System.Collections.Generic;

namespace Aragas.Core.Data
{
    public struct VarInt
    {
        private readonly int _value;

        public VarInt(int value) { _value = value; }

        public string ToString(IFormatProvider cultureInfo) => _value.ToString(cultureInfo);

        public override string ToString() => _value.ToString();

        public byte[] InByteArray()
        {
            var value = (uint) _value;

            var bytes = new List<byte>();
            while (true)
            {
                if ((value & 0xFFFFFF80u) == 0)
                {
                    bytes.Add((byte) value);
                    break;
                }
                bytes.Add((byte) (value & 0x7F | 0x80));
                value >>= 7;
            }

            return bytes.ToArray();
        }


        public static implicit operator VarInt(int value) => new VarInt(value);

        public static implicit operator int(VarInt value) => value._value;

        public static VarInt Parse(string @string, IFormatProvider provider) => int.Parse(@string, provider);
    }
}
