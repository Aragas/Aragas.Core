﻿using System;
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

        // -- String
        public string ReadString(int length = 0)
        {
            if(length == 0)
                length = ReadVarInt();

            var stringBytes = ReadByteArray(length);

            return Encoding.GetString(stringBytes, 0, stringBytes.Length);
        }

        // -- VarInt
        public VarInt ReadVarInt()
        {
            uint result = 0;
            int length = 0;

            while (true)
            {
                var current = ReadByte();
                result |= (current & 0x7Fu) << length++ * 7;

                if (length > 5)
                    throw new ProtobufReadingException("VarInt may not be longer than 28 bits.");
                
                if ((current & 0x80) != 128)
                    break;
            }
            return (int) result;
        }

        // -- Boolean
        public bool ReadBoolean()
        {
            return Convert.ToBoolean(ReadByte());
        }

        // -- SByte & Byte
        public sbyte ReadSByte()
        {
            return unchecked((sbyte)ReadByte());
        }
        public byte ReadByte()
        {
            return (byte)_stream.ReadByte();
        }

        // -- Short & UShort
        public short ReadShort()
        {
            var bytes = ReadByteArray(2);
            Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }
        public ushort ReadUShort()
        {
            return (ushort)((ReadByte() << 8) | ReadByte());
        }

        // -- Int & UInt
        public int ReadInt()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }
        public uint ReadUInt()
        {
            return (uint)(
                (ReadByte() << 24) |
                (ReadByte() << 16) |
                (ReadByte() << 8) |
                 ReadByte());
        }

        // -- Long & ULong
        public long ReadLong()
        {
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }
        public ulong ReadULong()
        {
            return unchecked(
                   ((ulong) ReadByte() << 56) |
                   ((ulong) ReadByte() << 48) |
                   ((ulong) ReadByte() << 40) |
                   ((ulong) ReadByte() << 32) |
                   ((ulong) ReadByte() << 24) |
                   ((ulong) ReadByte() << 16) |
                   ((ulong) ReadByte() << 8) |
                    (ulong) ReadByte());
        }

        // -- Floats
        public float ReadFloat()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        // -- Doubles
        public double ReadDouble()
        {
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }

        // -- StringArray
        public string[] ReadStringArray(int value)
        {
            var myStrings = new string[value];

            for (var i = 0; i < value; i++)
                myStrings[i] = ReadString();

            return myStrings;
        }

        // -- VarIntArray
        public int[] ReadVarIntArray(int value)
        {
            var myInts = new int[value];

            for (var i = 0; i < value; i++)
                myInts[i] = ReadVarInt();

            return myInts;
        }

        // -- IntArray
        public int[] ReadIntArray(int value)
        {
            var myInts = new int[value];

            for (var i = 0; i < value; i++)
                myInts[i] = ReadInt();

            return myInts;
        }

        // -- ByteArray
        public byte[] ReadByteArray(int value)
        {
            var myBytes = new byte[value];

            var bytesRead = _stream.Read(myBytes, 0, myBytes.Length);

            while (true)
            {
                if (bytesRead != value)
                {
                    var newSize = value - bytesRead;
                    var bytesRead1 = _stream.Read(myBytes, bytesRead - 1, newSize);

                    if (bytesRead1 != newSize)
                    {
                        value = newSize;
                        bytesRead = bytesRead1;
                    }
                    else break;
                }
                else break;
            }

            return myBytes;
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