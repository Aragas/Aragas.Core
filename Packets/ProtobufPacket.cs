using System;

using Aragas.Core.Interfaces;

namespace Aragas.Core.Packets
{
    public abstract class ProtobufPacket
    {
        public abstract Int32 ID { get; }

        public Int32 Origin { get; set; }

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
