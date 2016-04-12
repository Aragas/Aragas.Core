using Aragas.Core.Packets;

namespace Aragas.Core.PacketHandlers
{
    public class ContextFunc<TPacket> where TPacket : Packet
    {
        private readonly dynamic _instance;


        public ContextFunc(PacketHandler instance) { _instance = instance; }


        public TPacket Handle(dynamic packet) { return _instance.Handle(packet); }
        public ContextFunc<TPacket> SetContext(dynamic context) { _instance.Context = context; return this; }
    }
}