using Aragas.Core.Data;
using Aragas.Core.IO;

namespace Aragas.Core.Packets
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ProtobufPacket : Packet<VarInt, ProtobufPacket, ProtobufDataReader, ProtobufStream> { }
}