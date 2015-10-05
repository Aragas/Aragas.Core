﻿using System;
using System.Text;

using Aragas.Core.Data;
using Aragas.Core.Exceptions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

namespace Aragas.Core.IO
{
    public sealed class ProtobufStream : IPacketStream
    {
        public bool IsServer { get; }

        public bool Connected => _tcp != null && _tcp.Connected;
        public int DataAvailable => _tcp?.DataAvailable ?? 0;


        public bool EncryptionEnabled { get; private set; }

        private Encoding Encoding { get; } = Encoding.UTF8;


        private readonly INetworkTCPClient _tcp;

        private IAesStream _aesStream;
        private byte[] _buffer;

        public ProtobufStream(INetworkTCPClient tcp, bool isServer = false)
        {
            _tcp = tcp;
            IsServer = isServer;
        }


        public void Connect(string ip, ushort port)
        {
            _tcp.Connect(ip, port);
        }
        public void Disconnect()
        {
            _tcp.Disconnect();
        }


        public void InitializeEncryption(byte[] key)
        {
            _aesStream = new BouncyCastleAES(_tcp, key);

            EncryptionEnabled = true;
        }


        #region Write

        // -- String
        public void Write(string value, int length = 0)
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

            Write(final);
        }

        // -- VarInt
        public void Write(VarInt value)
        {
            Write(value.InByteArray());
        }

        // -- Boolean
        public void Write(bool value)
        {
            Write(Convert.ToByte(value));
        }

        // -- SByte & Byte
        public void Write(sbyte value)
        {
            Write(unchecked((byte) value));
        }
        public void Write(byte value)
        {
            Write(new[] { value });
        }

        // -- Short & UShort
        public void Write(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            Write(bytes);
        }
        public void Write(ushort value)
        {
            Write(new[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Int & UInt
        public void Write(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            Write(bytes);
        }
        public void Write(uint value)
        {
            Write(new[]
            {
                (byte) ((value & 0xFF000000) >> 24),
                (byte) ((value & 0xFF0000) >> 16),
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Long & ULong
        public void Write(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            Write(bytes);
        }
        public void Write(ulong value)
        {
            Write(new[]
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
        public void Write(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            Write(bytes);
        }

        // -- Double
        public void Write(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            Write(bytes);
        }

        // -- StringArray
        public void Write(string[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                Write(value[i]);
        }

        // -- VarIntArray
        public void Write(VarInt[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                Write(value[i]);
        }

        // -- IntArray
        public void Write(int[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                Write(value[i]);
        }

        // -- ByteArray
        public void Write(byte[] value)
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


        #region Read

        public byte ReadByte()
        {
            var buffer = new byte[1];
            
            Receive(buffer, 0, buffer.Length);
            
            return buffer[0];
        }

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

        public byte[] ReadByteArray(int value)
        {
            var result = new byte[value];
            if (value == 0) return result;
            int n = value;
            while (true)
            {
                n -= Receive(result, value - n, n);
                if (n == 0)
                    break;
            }
            return result;
        }

        #endregion Read


        private void Send(byte[] buffer, int offset, int count)
        {
            if (EncryptionEnabled)
                _aesStream.Write(buffer, offset, count);
            else
                _tcp.Send(buffer, offset, count);
        }
        private int Receive(byte[] buffer, int offset, int count)
        {
            if (EncryptionEnabled)
                return _aesStream.Read(buffer, offset, count);
            else
                return _tcp.Receive(buffer, offset, count);
        }

        public void SendPacket(ref ProtobufPacket packet)
        {
            Write(packet.ID);
            Write(packet.Origin);
            packet.WritePacket(this);
            Purge();
        }


        private void Purge()
        {
            var lenBytes = new VarInt(_buffer.Length).InByteArray();
            var tempBuff = new byte[_buffer.Length + lenBytes.Length];
            
            Array.Copy(lenBytes, 0, tempBuff, 0, lenBytes.Length);
            Array.Copy(_buffer, 0, tempBuff, lenBytes.Length, _buffer.Length);

            Send(tempBuff, 0, tempBuff.Length);

            _buffer = null;
        }


        public void Dispose()
        {
            _tcp?.Disconnect().Dispose();

            _aesStream?.Dispose();

            _buffer = null;
        }
    }
}