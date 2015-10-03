using System;

namespace Aragas.Core.Wrappers
{
    public interface INetworkTCPServer : IDisposable
    {
        UInt16 Port { get; }
        Boolean AvailableClients { get; }

        void Start();
        void Stop();

        INetworkTCPClient AcceptNetworkTCPClient();

        INetworkTCPServer NewInstance(UInt16 port);
    }

    public static class NetworkTCPServerWrapper
    {
        private static INetworkTCPServer _instance;
        public static INetworkTCPServer Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static INetworkTCPServer NewInstance(ushort port) { return Instance.NewInstance(port); }
    }
}
