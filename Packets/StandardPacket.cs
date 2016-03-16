using Aragas.Core.IO;

namespace Aragas.Core.Packets
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class StandardPacket : Packet<int, ProtobufPacket, StandardDataReader, StandardStream> { }
}