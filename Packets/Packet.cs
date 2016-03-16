using Aragas.Core.IO;

namespace Aragas.Core.Packets
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Packet { }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Packet<TIDType, TPacketType, TReader, TWriter> : Packet where TPacketType : Packet where TReader : PacketDataReader where TWriter : PacketStream
    {
        public abstract TIDType ID { get; }

        /// <summary>
        /// Read packet from IPacketDataReader.
        /// </summary>
        public abstract TPacketType ReadPacket(TReader reader);

        /// <summary>
        /// Write packet to IPacketStream.
        /// </summary>
        public abstract TPacketType WritePacket(TWriter writer);
    }
}
