using Aragas.Core.Packets;

namespace Aragas.Core.PacketHandlers
{
    public abstract class PacketHandler { }
    public abstract class PacketHandler<TRequestPacket, TReplyPacket, TContext> : PacketHandler where TRequestPacket : Packet where TReplyPacket : Packet where TContext : IPacketHandlerContext
    {
        public TContext Context { protected get; set; }

        public abstract TReplyPacket Handle(TRequestPacket packet);
    }
}
