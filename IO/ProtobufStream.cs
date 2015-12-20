﻿using System;
using System.Text;

using Aragas.Core.Data;
using Aragas.Core.Exceptions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

namespace Aragas.Core.IO
{
    public class ProtobufStream : PacketStream
    {
        public override bool IsServer { get; }

        public override string Host => TCP.IP;
        public override ushort Port => TCP.Port;
        public override bool Connected => TCP != null && TCP.Connected;
        public override int DataAvailable => TCP?.DataAvailable ?? 0;


        public override bool EncryptionEnabled { get; protected set; }

        private Encoding Encoding { get; } = Encoding.UTF8;


        protected ITCPClient TCP { get; }

        private IAesStream _aesStream;
        protected byte[] _buffer;

        public ProtobufStream(ITCPClient tcp, bool isServer = false)
        {
            TCP = tcp;
            IsServer = isServer;
        }


        public override void Connect(string ip, ushort port)
        {
            TCP.Connect(ip, port);
        }
        public override void Disconnect()
        {
            TCP.Disconnect();
        }


        public override void InitializeEncryption(byte[] key)
        {
            _aesStream = new BouncyCastleAES(TCP, key);

            EncryptionEnabled = true;
        }


        #region Write


        // -- String
        public override void Write(string value, int length = 0)
        {
            byte[] lengthBytes;
            byte[] final;

            if (length == 0)
            {
                length = value.Length;

                lengthBytes = new VarInt(value.Length).InByteArray();
                final = new byte[value.Length + lengthBytes.Length];
            }
            else
            {
                lengthBytes = new VarInt(length).InByteArray();
                final = new byte[length + lengthBytes.Length];
            }

            Buffer.BlockCopy(lengthBytes, 0, final, 0, lengthBytes.Length);
            Buffer.BlockCopy(Encoding.GetBytes(value), 0, final, lengthBytes.Length, length);

            ToBuffer(final);
        }

        // -- VarInt
        public override void Write(VarInt value)
        {
            ToBuffer(value.InByteArray());
        }

        // -- Boolean
        public override void Write(bool value)
        {
            Write(Convert.ToByte(value));
        }

        // -- SByte & Byte
        public override void Write(sbyte value)
        {
            Write(unchecked((byte) value));
        }
        public override void Write(byte value)
        {
            ToBuffer(new[] { value });
        }

        // -- Short & UShort
        public override void Write(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        public override void Write(ushort value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Int & UInt
        public override void Write(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        public override void Write(uint value)
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
        public override void Write(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        public override void Write(ulong value)
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
        public override void Write(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }

        // -- Double
        public override void Write(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }

        // -- StringArray
        public override void Write(string[] value)
        {
            var length = value.Length;
            Write(new VarInt(length));

            for (var i = 0; i < length; i++)
                Write(value[i]);
        }

        // -- VarIntArray
        public override void Write(VarInt[] value)
        {
            var length = value.Length;
            Write(new VarInt(length));

            for (var i = 0; i < length; i++)
                Write(value[i]);
        }

        // -- IntArray
        public override void Write(int[] value)
        {
            var length = value.Length;
            Write(new VarInt(length));

            for (var i = 0; i < length; i++)
                Write(value[i]);
        }

        // -- ByteArray
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
        public override void Write(byte[] value)
        {
            var length = value.Length;
            Write(new VarInt(length));

            ToBuffer(value);
        }

        #endregion Write


        #region Read

        public override byte ReadByte()
        {
            return Receive(1)[0];
        }

        public override VarInt ReadVarInt()
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

        public override byte[] ReadByteArray(int length)
        {
            return Receive(length);
        }

        #endregion Read

        protected void Send(byte[] buffer)
        {
            if (EncryptionEnabled)
                _aesStream.EncryptByteArray(buffer);
            else
                TCP.WriteByteArray(buffer);
        }
        private byte[] Receive(int length)
        {
            if (EncryptionEnabled)
                return _aesStream.DecryptByteArray(length);
            else
                return TCP.ReadByteArray(length);
        }

        public override void SendPacket(ref ProtobufPacket packet)
        {
            Write(packet.ID);
            packet.WritePacket(this);
            Purge();
        }


        protected virtual void Purge()
        {
            var lenBytes = new VarInt(_buffer.Length).InByteArray();
            var tempBuff = new byte[_buffer.Length + lenBytes.Length];
            
            Array.Copy(lenBytes, 0, tempBuff, 0, lenBytes.Length);
            Array.Copy(_buffer, 0, tempBuff, lenBytes.Length, _buffer.Length);

            Send(tempBuff);

            _buffer = null;
        }


        public override void Dispose()
        {
            TCP?.Disconnect().Dispose();

            _aesStream?.Dispose();

            _buffer = null;
        }
    }
}