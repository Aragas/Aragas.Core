using System;
using System.IO;

namespace Aragas.Core.Wrappers
{
    public interface ITCPClient : IDisposable
    {
        string IP { get; }
        bool Connected { get; }
        int DataAvailable { get; }

        int RefreshConnectionInfoTime { get; set; }


        ITCPClient Connect(string ip, ushort port);
        ITCPClient Disconnect();

        void WriteByteArray(byte[] array);
        byte[] ReadByteArray(int length);

        Stream GetStream();
    }

    public interface ITCPClientWrapper
    {
        ITCPClient CreateTCPClient();
    }

    public static class TCPClientWrapper
    {
        private static ITCPClientWrapper _instance;
        public static ITCPClientWrapper Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static ITCPClient CreateTCPClient() { return Instance.CreateTCPClient(); }
    }
}
