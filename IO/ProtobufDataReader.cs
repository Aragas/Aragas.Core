using System;
using System.IO;
using System.Text;

using Aragas.Core.Data;
using Aragas.Core.Exceptions;
using Aragas.Core.Interfaces;

namespace Aragas.Core.IO
{
    public sealed class ProtobufDataReader : IPacketDataReader
    {
        public bool IsServer { get; }

        private Encoding Encoding { get; } = Encoding.UTF8;

        private readonly Stream _stream;
        
        public ProtobufDataReader(Stream stream, bool isServer = false)
        {
            _stream = stream;
            IsServer = isServer;
        }

        public ProtobufDataReader(byte[] data, bool isServer = false)
        {
            _stream = new MemoryStream(data);
            IsServer = isServer;
        }


        // -- Anything
        public T Read<T>(T value = default(T), int length = 0)
        {
            var type = typeof(T);

            if (length > 0)
            {
                if (type == typeof(string))
                    return (T) Convert.ChangeType(ReadString(length), typeof(T));

                if (type == typeof(string[]))
                    return (T) Convert.ChangeType(ReadStringArray(length), typeof(T));
                if (type == typeof(VarInt[]))
                    return (T) Convert.ChangeType(ReadVarIntArray(length), typeof(T));
                if (type == typeof(int[]))
                    return (T) Convert.ChangeType(ReadIntArray(length), typeof(T));
                if (type == typeof(byte[]))
                    return (T) Convert.ChangeType(ReadByteArray(length), typeof(T));

                return value;
            }


            if (type == typeof(string))
                return (T) Convert.ChangeType(ReadString(), typeof(T));

            if (type == typeof(VarInt))
                return (T) Convert.ChangeType(ReadVarInt(), typeof(T));


            if (type == typeof(bool))
                return (T) Convert.ChangeType(ReadBoolean(), typeof(T));

            if (type == typeof(sbyte))
                return (T) Convert.ChangeType(ReadSByte(), typeof(T));
            if (type == typeof(byte))
                return (T) Convert.ChangeType(ReadByte(), typeof(T));

            if (type == typeof(short))
                return (T) Convert.ChangeType(ReadShort(), typeof(T));
            if (type == typeof(ushort))
                return (T) Convert.ChangeType(ReadUShort(), typeof(T));

            if (type == typeof(int))
                return (T) Convert.ChangeType(ReadInt(), typeof(T));
            if (type == typeof(uint))
                return (T) Convert.ChangeType(ReadUInt(), typeof(T));

            if (type == typeof(long))
                return (T) Convert.ChangeType(ReadLong(), typeof(T));
            if (type == typeof(ulong))
                return (T) Convert.ChangeType(ReadULong(), typeof(T));

            if (type == typeof(float))
                return (T) Convert.ChangeType(ReadFloat(), typeof(T));

            if (type == typeof(double))
                return (T) Convert.ChangeType(ReadDouble(), typeof(T));

            if (type == typeof(string[]))
                return (T) Convert.ChangeType(ReadStringArray(), typeof(T));
            if (type == typeof(VarInt[]))
                return (T) Convert.ChangeType(ReadVarIntArray(), typeof(T));
            if (type == typeof(int[]))
                return (T) Convert.ChangeType(ReadIntArray(), typeof(T));
            if (type == typeof(byte[]))
                return (T) Convert.ChangeType(ReadByteArray(), typeof(T));


            return value;
        }

        // -- String
        private string ReadString(int length = 0)
        {
            if (length == 0)
                length = Read<VarInt>();

            var stringBytes = Read<byte[]>(null, length);

            return Encoding.GetString(stringBytes, 0, stringBytes.Length);
        }

        // -- VarInt
        private VarInt ReadVarInt()
        {
            uint result = 0;
            int length = 0;

            while (true)
            {
                var current = Read<byte>();
                result |= (current & 0x7Fu) << length++ * 7;

                if (length > 5)
                    throw new ProtobufReadingException("VarInt may not be longer than 28 bits.");

                if ((current & 0x80) != 128)
                    break;
            }
            return (int) result;
        }

        // -- Boolean
        private bool ReadBoolean()
        {
            return Convert.ToBoolean(Read<byte>());
        }

        // -- SByte & Byte
        private sbyte ReadSByte()
        {
            return unchecked((sbyte) Read<byte>());
        }
        private byte ReadByte()
        {
            return (byte) _stream.ReadByte();
        }

        // -- Short & UShort
        private short ReadShort()
        {
            var bytes = Read<byte[]>(null, 2);
            Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }
        private ushort ReadUShort()
        {
            return (ushort) ((Read<byte>() << 8) | Read<byte>());
        }

        // -- Int & UInt
        private int ReadInt()
        {
            var bytes = Read<byte[]>(null, 4);
            Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }
        private uint ReadUInt()
        {
            return (uint) (
                (Read<byte>() << 24) |
                (Read<byte>() << 16) |
                (Read<byte>() << 8) |
                (Read<byte>()));
        }

        // -- Long & ULong
        private long ReadLong()
        {
            var bytes = Read<byte[]>(null, 8);
            Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }
        private ulong ReadULong()
        {
            return unchecked(
                   ((ulong) Read<byte>() << 56) |
                   ((ulong) Read<byte>() << 48) |
                   ((ulong) Read<byte>() << 40) |
                   ((ulong) Read<byte>() << 32) |
                   ((ulong) Read<byte>() << 24) |
                   ((ulong) Read<byte>() << 16) |
                   ((ulong) Read<byte>() << 8) |
                    (ulong) Read<byte>());
        }

        // -- Floats
        private float ReadFloat()
        {
            var bytes = Read<byte[]>(null, 4);
            Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        // -- Doubles
        private double ReadDouble()
        {
            var bytes = Read<byte[]>(null, 8);
            Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }

        // -- StringArray
        private string[] ReadStringArray()
        {
            var length = Read<VarInt>();
            return Read<string[]>(null, length);
        }
        private string[] ReadStringArray(int length)
        {
            var myStrings = new string[length];

            for (var i = 0; i < length; i++)
                myStrings[i] = Read<string>();

            return myStrings;
        }

        // -- VarIntArray
        private VarInt[] ReadVarIntArray()
        {
            var length = Read<VarInt>();
            return Read<VarInt[]>(null, length);
        }
        private VarInt[] ReadVarIntArray(int length)
        {
            var myInts = new VarInt[length];

            for (var i = 0; i < length; i++)
                myInts[i] = Read<VarInt>();

            return myInts;
        }

        // -- IntArray
        private int[] ReadIntArray()
        {
            var length = Read<VarInt>();
            return Read<int[]>(null, length);
        }
        private int[] ReadIntArray(int length)
        {
            var myInts = new int[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadInt();

            return myInts;
        }

        // -- ByteArray
        private byte[] ReadByteArray()
        {
            var length = Read<VarInt>();
            return Read<byte[]>(null, length);
        }
        private byte[] ReadByteArray(int length)
        {
            if (length == 0)
                return new byte[length];

            var msg = new byte[length];
            var readSoFar = 0;
            while (readSoFar < length)
            {
                var read = _stream.Read(msg, readSoFar, msg.Length - readSoFar);
                readSoFar += read;
                if (read == 0)
                    break;   // connection was broken
            }

            return msg;
        }


        public int BytesLeft()
        {
            return (int)(_stream.Length - _stream.Position);
        }


        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}
