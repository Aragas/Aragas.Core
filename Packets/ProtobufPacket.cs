using System;
using Aragas.Core.Data;
using Aragas.Core.Interfaces;

namespace Aragas.Core.Packets
{
    public abstract class ProtobufPacket
    {
        public abstract VarInt ID { get; }

        public VarInt Origin { get; set; } = new VarInt(0);

        /// <summary>
        /// Read packet from IPacketDataReader.
        /// </summary>
        public abstract ProtobufPacket ReadPacket(IPacketDataReader reader);

        /// <summary>
        /// Write packet to IPacketStream.
        /// </summary>
        public abstract ProtobufPacket WritePacket(IPacketStream writer);
    }
}
