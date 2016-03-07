using System.IO;

using Aragas.Core.Data;
using Aragas.Core.Packets;

namespace Aragas.Core.IO
{
    public interface IEncryptedStream
    {
        bool EncryptionEnabled { get; }

        void InitializeEncryption(byte[] key);
    }

    public interface ICompressedStream
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public abstract partial class PacketStream
    {
        public abstract bool IsServer { get; }
        public abstract string Host { get; }
        public abstract ushort Port { get; }
        public abstract bool Connected { get; }
        public abstract int DataAvailable { get; }

        protected abstract Stream BaseStream { get; }

        public abstract void Connect(string ip, ushort port);
        public abstract void Disconnect();

        public abstract void SendPacket<TIDType, TPacketType>(ref Packet<TIDType, TPacketType> packet) where TPacketType : Packet;


        #region Write

        public abstract void Write(string value, int length = 0);

        public abstract void Write(VarShort value);
        public abstract void Write(VarZShort value);
        public abstract void Write(VarInt value);
        public abstract void Write(VarZInt value);
        public abstract void Write(VarLong value);
        public abstract void Write(VarZLong value);

        public abstract void Write(bool value);

        public abstract void Write(sbyte value);
        public abstract void Write(byte value);

        public abstract void Write(short value);
        public abstract void Write(ushort value);

        public abstract void Write(int value);
        public abstract void Write(uint value);

        public abstract void Write(long value);
        public abstract void Write(ulong value);

        public abstract void Write(double value);

        public abstract void Write(float value);


        public abstract void Write(string[] value);

        public abstract void Write(int[] value);

        public abstract void Write(VarShort[] value);
        public abstract void Write(VarZShort[] value);
        public abstract void Write(VarInt[] value);
        public abstract void Write(VarZInt[] value);
        public abstract void Write(VarLong[] value);
        public abstract void Write(VarZLong[] value);

        public abstract void Write(byte[] value);

        #endregion Write


        #region Read

        public abstract VarInt ReadVarInt();

        #endregion Read
    }
}
