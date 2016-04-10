using Aragas.Core.Packets;

namespace Aragas.Core.PacketHandlers
{
    public class ContextFunc<TPacket> where TPacket : Packet
    {
        private readonly dynamic _instance;


        public ContextFunc(PacketHandler instance) { _instance = instance; }


        // First MethodInfo.Invoke was used, but Exception handling is there broken.
        public TPacket Handle(dynamic packet) { return _instance.Handle(packet); }
        public void SetContext(dynamic context) { _instance.Context = context; }
    }
}