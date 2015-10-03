using System;
using System.Numerics;

using Aragas.Core.Data;
using Aragas.Core.Packets;

namespace Aragas.Core.Interfaces
{
    public interface IPacketStreamWrite
    {
        void WriteString(String value, Int32 length = 0);

        void WriteVarInt(VarInt value);

        void WriteBoolean(Boolean value);

        void WriteSByte(SByte value);
        void WriteByte(Byte value);

        void WriteShort(Int16 value);
        void WriteUShort(UInt16 value);

        void WriteInt(Int32 value);
        void WriteUInt(UInt32 value);

        void WriteLong(Int64 value);
        void WriteULong(UInt64 value);

        void WriteBigInteger(BigInteger value);
        void WriteUBigInteger(BigInteger value);

        void WriteDouble(Double value);

        void WriteFloat(Single value);


        void WriteStringArray(params String[] value);

        void WriteVarIntArray(params Int32[] value);

        void WriteIntArray(params Int32[] value);

        void WriteByteArray(params Byte[] value);
    }

    public interface IPacketStreamRead
    {
        Byte ReadByte();

        VarInt ReadVarInt();

        Byte[] ReadByteArray(Int32 value);

        String ReadLine();
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
