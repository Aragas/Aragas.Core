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
    public abstract class Packet<TIDType, TPacketType> : Packet where TPacketType : Packet
    {
        public abstract TIDType ID { get; }

        /// <summary>
        /// Read packet from IPacketDataReader.
        /// </summary>
        public abstract TPacketType ReadPacket(PacketDataReader reader);

        /// <summary>
        /// Write packet to IPacketStream.
        /// </summary>
        public abstract TPacketType WritePacket(PacketStream writer);
    }
}
