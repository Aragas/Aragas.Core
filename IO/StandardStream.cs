using System;
using System.IO;
using System.Text;

using Aragas.Core.Data;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

namespace Aragas.Core.IO
{
    /// <summary>
    /// Stream that uses int for length encoding.
    /// </summary>
    public class StandardStream : PacketStream
    {
        public override bool IsServer { get; }

        public override string Host => TCPClient.IP;
        public override ushort Port => TCPClient.Port;
        public override bool Connected => TCPClient != null && TCPClient.Connected;
        public override int DataAvailable => TCPClient?.DataAvailable ?? 0;


        private Encoding Encoding { get; } = Encoding.UTF8;


        private ITCPClient TCPClient { get; }

        protected override Stream BaseStream => TCPClient.GetStream();
        protected MemoryStream Buffer { get; } = new MemoryStream();


        public StandardStream(ITCPClient tcp, bool isServer = false)
        {
            TCPClient = tcp;
            IsServer = isServer;
        }


        public override void Connect(string ip, ushort port) { TCPClient.Connect(ip, port); }
        public override void Disconnect() { TCPClient.Disconnect(); }


        #region Write

        public override void Write(VarShort value) { throw new NotSupportedException(); }
        public override void Write(VarZShort value) { throw new NotSupportedException(); }

        public override void Write(VarInt value) { throw new NotSupportedException(); }
        public override void Write(VarZInt value) { throw new NotSupportedException(); }

        public override void Write(VarLong value) { throw new NotSupportedException(); }
        public override void Write(VarZLong value) { throw new NotSupportedException(); }

        public override void Write(VarShort[] value) { throw new NotSupportedException(); }
        public override void Write(VarZShort[] value) { throw new NotSupportedException(); }

        public override void Write(VarInt[] value) { throw new NotSupportedException(); }
        public override void Write(VarZInt[] value) { throw new NotSupportedException(); }

        public override void Write(VarLong[] value) { throw new NotSupportedException(); }
        public override void Write(VarZLong[] value) { throw new NotSupportedException(); }


        // -- String
        public override void Write(string value, int length = 0)
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
        public override void Write(bool value) { Write(Convert.ToByte(value)); }

        // -- SByte & Byte
        public override void Write(sbyte value) { Write(unchecked((byte) value)); }
        public override void Write(byte value) { ToBuffer(new[] { value }); }

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
            Write(value.Length);

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- IntArray
        public override void Write(int[] value)
        {
            Write(value.Length);

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- ByteArray
        protected void ToBuffer(byte[] value) { Buffer.Write(value, 0, value.Length); }
        public override void Write(byte[] value)
        {
            Write(value.Length);

            ToBuffer(value);
        }

        #endregion Write


        #region Read

        public override VarInt ReadVarInt() { throw new NotImplementedException(); }

        #endregion Read
        

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

        public override void SendPacket<TIDType, TPacketType>(ref Packet<TIDType, TPacketType> packet)
        {
            var standartPacket = packet as StandardPacket;
            Write(standartPacket.ID);
            standartPacket.WritePacket(this);
            Purge();
        }


        protected virtual void Purge()
        {
            var array = Buffer.ToArray();

            var lenBytes = BitConverter.GetBytes(array.Length);
            var tempBuff = new byte[array.Length + lenBytes.Length];

            Array.Copy(lenBytes, 0, tempBuff, 0, lenBytes.Length);
            Array.Copy(array, 0, tempBuff, lenBytes.Length, array.Length);

            Send(array);

            Buffer.SetLength(0);
        }
    }
}