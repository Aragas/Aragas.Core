using System;

using Aragas.Core.Data;
using Aragas.Core.Packets;

namespace Aragas.Core.Interfaces
{
    public interface IPacketStreamWrite
    {
        void Write(String value, Int32 length = 0);

        void Write(VarInt value);

        void Write(Boolean value);

        void Write(SByte value);
        void Write(Byte value);

        void Write(Int16 value);
        void Write(UInt16 value);

        void Write(Int32 value);
        void Write(UInt32 value);

        void Write(Int64 value);
        void Write(UInt64 value);

        void Write(Double value);

        void Write(Single value);


        void Write(String[] value);

        void Write(Int32[] value);

        void Write(VarInt[] value);

        void Write(Byte[] value);
    }

    public interface IPacketStreamRead
    {
        Byte ReadByte();

        VarInt ReadVarInt();

        Byte[] ReadByteArray(Int32 value);
    }
    
    public interface IPacketStreamConnection
    {
        void Connect(String ip, UInt16 port);
        void Disconnect();

        void SendPacket(ref ProtobufPacket packet);
    }

    public interface IPacketStreamStatus
    {
        Boolean Connected { get; }
        Int32 DataAvailable { get; }

        Boolean EncryptionEnabled { get; }

        void InitializeEncryption(Byte[] key);
    }

    /// <summary>
    /// Object that reads VarInt (or Byte) and ByteArray for handling Data later 
    /// and writes any data from packet to user-defined object, that will interact with Minecraft Server.
    /// </summary>
    public interface IPacketStream : IPacketStreamWrite, IPacketStreamRead, IPacketStreamConnection, IPacketStreamStatus, IDisposable
    {
        Boolean IsServer { get; }
    }
}
