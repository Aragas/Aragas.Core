using System;

using Aragas.Core.Data;
using Aragas.Core.Packets;

namespace Aragas.Core.IO
{
    /// <summary>
    /// Object that reads VarInt (or Byte) and ByteArray for handling Data later 
    /// and writes any data from packet to user-defined object, that will interact with Minecraft Server.
    /// </summary>
    public abstract class PacketStream : IDisposable
    {
        public abstract Boolean IsServer { get; }
        public abstract String Host { get; }
        public abstract UInt16 Port { get; }
        public abstract Boolean Connected { get; }
        public abstract Int32 DataAvailable { get; }
        public abstract Boolean EncryptionEnabled { get; protected set; }

        public abstract void Connect(String ip, UInt16 port);
        public abstract void Disconnect();

        public abstract void SendPacket(ref ProtobufPacket packet);

        public abstract void InitializeEncryption(Byte[] key);


        #region Write

        public abstract void Write(String value, Int32 length = 0);

        public abstract void Write(VarInt value);

        public abstract void Write(Boolean value);

        public abstract void Write(SByte value);
        public abstract void Write(Byte value);

        public abstract void Write(Int16 value);
        public abstract void Write(UInt16 value);

        public abstract void Write(Int32 value);
        public abstract void Write(UInt32 value);

        public abstract void Write(Int64 value);
        public abstract void Write(UInt64 value);

        public abstract void Write(Double value);

        public abstract void Write(Single value);


        public abstract void Write(String[] value);

        public abstract void Write(Int32[] value);

        public abstract void Write(VarInt[] value);

        public abstract void Write(Byte[] value);

        #endregion Write


        #region Read

        public abstract Byte ReadByte();

        public abstract VarInt ReadVarInt();

        public abstract Byte[] ReadByteArray(Int32 length);

        #endregion Read


        public abstract void Dispose();
    }
}
