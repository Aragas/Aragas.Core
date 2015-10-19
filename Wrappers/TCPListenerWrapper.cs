using System;

namespace Aragas.Core.Wrappers
{
    public interface ITCPListener : IDisposable
    {
        ushort Port { get; }
        bool AvailableClients { get; }

        void Start();
        void Stop();

        ITCPClient AcceptNetworkTCPClient();
    }

    public interface ITCPListenerWrapper
    {
        ITCPListener CreateTCPListener(ushort port);
    }

    public static class TCPListenerWrapper
    {
        private static ITCPListenerWrapper _instance;
        public static ITCPListenerWrapper Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static ITCPListener CreateTCPListener(ushort port) { return Instance.CreateTCPListener(port); }
    }
}
