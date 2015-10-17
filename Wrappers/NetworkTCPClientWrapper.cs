using System;
using System.IO;

namespace Aragas.Core.Wrappers
{
    public interface INetworkTCPClient : IDisposable
    {
        int RefreshConnectionInfoTime { get; set; }

        String IP { get; }
        Boolean Connected { get; }
        Int32 DataAvailable { get; }


        INetworkTCPClient Connect(String ip, UInt16 port);
        INetworkTCPClient Disconnect();


        void Send(Byte[] bytes, Int32 offset, Int32 count);
        Int32 Receive(Byte[] buffer, Int32 offset, Int32 count);


        Stream GetStream();


        INetworkTCPClient NewInstance();
    }

    public static class NetworkTCPClientWrapper
    {
        private static INetworkTCPClient _instance;
        public static INetworkTCPClient Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static INetworkTCPClient NewInstance() { return Instance.NewInstance(); }
    }
}
