﻿using System;
using System.IO;
using System.Text;

using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

namespace Aragas.Core.IO
{
    /// <summary>
    /// Stream that uses int for length encoding.
    /// </summary>
    public sealed class StandardStream : PacketStream
    {
        public override bool IsServer { get; }

        public override string Host => TCPClient.IP;
        public override ushort Port => TCPClient.Port;
        public override bool Connected => TCPClient != null && TCPClient.Connected;
        public override int DataAvailable => TCPClient?.DataAvailable ?? 0;


        private Encoding Encoding { get; } = Encoding.UTF8;


        private ITCPClient TCPClient { get; }

        protected override Stream BaseStream => TCPClient.GetStream();
        private byte[] _buffer;


        public StandardStream(ITCPClient tcp, bool isServer = false)
        {
            TCPClient = tcp;
            IsServer = isServer;
        }


        public override void Connect(string ip, ushort port) { TCPClient.Connect(ip, port); }
        public override void Disconnect() { TCPClient.Disconnect(); }


        #region Write

        // -- Anything
        public override void Write<T>(T value = default(T))
        {
            var type = typeof(T);

            if (type == typeof (string))
                WriteString((string) (object) value);
            
            if (type == typeof (bool))
                WriteBoolean((bool) (object) value);

            if (type == typeof (sbyte))
                WriteSByte((sbyte) (object) value);
            if (type == typeof (byte))
                WriteByte((byte) (object) value);

            if (type == typeof (short))
                WriteShort((short) (object) value);
            if (type == typeof (ushort))
                WriteUShort((ushort) (object) value);

            if (type == typeof (int))
                WriteInt((int) (object) value);
            if (type == typeof (uint))
                WriteUInt((uint) (object) value);

            if (type == typeof (long))
                WriteLong((long) (object) value);
            if (type == typeof (ulong))
                WriteULong((ulong) (object) value);

            if (type == typeof (float))
                WriteFloat((float) (object) value);

            if (type == typeof (double))
                WriteDouble((double) (object) value);


            if (ExtendWriteContains(type))
                ExtendWriteExecute(this, value);


            if (type == typeof (string[]))
                WriteStringArray((string[]) (object) value);
            if (type == typeof (int[]))
                WriteIntArray((int[]) (object) value);
            if (type == typeof (byte[]))
                WriteByteArray((byte[]) (object) value);
        }

        // -- String
        private void WriteString(string value, int length = 0)
        {
            byte[] lengthBytes;
            byte[] final;

            if (length == 0)
            {
                length = value.Length;

                lengthBytes = BitConverter.GetBytes(value.Length);
                final = new byte[value.Length + lengthBytes.Length];
            }
            else
            {
                lengthBytes = BitConverter.GetBytes(length);
                final = new byte[length + lengthBytes.Length];
            }

            System.Buffer.BlockCopy(lengthBytes, 0, final, 0, lengthBytes.Length);
            System.Buffer.BlockCopy(Encoding.GetBytes(value), 0, final, lengthBytes.Length, length);

            ToBuffer(final);
        }

        // -- Boolean
        private void WriteBoolean(bool value) { Write(Convert.ToByte(value)); }

        // -- SByte & Byte
        private void WriteSByte(sbyte value) { Write(unchecked((byte) value)); }
        private void WriteByte(byte value) { ToBuffer(new[] { value }); }

        // -- Short & UShort
        private void WriteShort(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        private void WriteUShort(ushort value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Int & UInt
        private void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        private void WriteUInt(uint value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF000000) >> 24),
                (byte) ((value & 0xFF0000) >> 16),
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Long & ULong
        private void WriteLong(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        private void WriteULong(ulong value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF00000000000000) >> 56),
                (byte) ((value & 0xFF000000000000) >> 48),
                (byte) ((value & 0xFF0000000000) >> 40),
                (byte) ((value & 0xFF00000000) >> 32),
                (byte) ((value & 0xFF000000) >> 24),
                (byte) ((value & 0xFF0000) >> 16),
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Float
        private void WriteFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }

        // -- Double
        private void WriteDouble(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }

        // -- StringArray
        private void WriteStringArray(string[] value)
        {
            Write(value.Length);

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- IntArray
        private void WriteIntArray(int[] value)
        {
            Write(value.Length);

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- ByteArray
        private void WriteByteArray(byte[] value)
        {
            Write(value.Length);

            ToBuffer(value);
        }


        private void ToBuffer(byte[] value)
        {
            if (_buffer != null)
            {
                Array.Resize(ref _buffer, _buffer.Length + value.Length);
                Array.Copy(value, 0, _buffer, _buffer.Length - value.Length, value.Length);
            }
            else
                _buffer = value;
        }

        #endregion Write


        public void Send(byte[] buffer)
        {
            TCPClient.Write(buffer, 0, buffer.Length);
        }
        public byte[] Receive(int length)
        {
            var buffer = new byte[length];
            TCPClient.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public override void SendPacket(Packet packet)
        {
            var standartPacket = packet as StandardPacket;
            Write(standartPacket.ID);
            standartPacket.WritePacket(this);
            Purge();
        }


        private void Purge()
        {
            var lenBytes = BitConverter.GetBytes(_buffer.Length);
            var tempBuff = new byte[_buffer.Length + lenBytes.Length];

            Array.Copy(lenBytes, 0, tempBuff, 0, lenBytes.Length);
            Array.Copy(_buffer, 0, tempBuff, lenBytes.Length, _buffer.Length);

            Send(_buffer);

            _buffer = null;
        }


        public override void Dispose()
        {
            _buffer = null;
        }
    }
}