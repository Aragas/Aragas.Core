using System;
using System.IO;

namespace Aragas.Core.Wrappers
{
    public interface ITCPClient : IDisposable
    {
        String IP { get; }
        UInt16 Port { get; }
        Boolean Connected { get; }
        Int32 DataAvailable { get; }

        Int32 RefreshConnectionInfoTime { get; set; }


        ITCPClient Connect(String ip, UInt16 port);
        ITCPClient Disconnect();

        int Write(byte[] buffer, int offset, int count);
        int Read(byte[] buffer, int offset, int count);

        Stream GetStream();
    }

    public interface ITCPClientFactory
    {
        ITCPClient Create();
    }

    public static class TCPClientWrapper
    {
        private static ITCPClientFactory _instance;
        public static ITCPClientFactory Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static ITCPClient Create() { return Instance.Create(); }
    }
}
