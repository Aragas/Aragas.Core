using System;
using System.Collections.Generic;
using System.IO;

using Aragas.Core.Packets;

namespace Aragas.Core.IO
{
    public interface IEncryptedStream
    {
        bool EncryptionEnabled { get; }

        void InitializeEncryption(byte[] key);
    }

    public interface ICompressedStream
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public abstract partial class PacketStream : IDisposable
    {
        public abstract bool IsServer { get; }
        public abstract string Host { get; }
        public abstract ushort Port { get; }
        public abstract bool Connected { get; }
        public abstract int DataAvailable { get; }

        protected abstract Stream BaseStream { get; }

        public abstract void Connect(string ip, ushort port);
        public abstract void Disconnect();

        public abstract void SendPacket(Packet packet);


        #region ExtendWrite

        private static readonly Dictionary<int, Action<PacketStream, object>> WriteExtendedList = new Dictionary<int, Action<PacketStream, object>>();

        public static void ExtendWrite<T>(Action<PacketStream, T> action) { WriteExtendedList.Add(typeof(T).GetHashCode(), Change(action)); }

        private static Action<PacketStream, object> Change<T>(Action<PacketStream, T> action) => action == null ? (Action<PacketStream, object>) null : ((stream, value) => action(stream, (T) value));

        protected static bool ExtendWriteContains<T>() => ExtendWriteContains(typeof(T));

        protected static bool ExtendWriteContains(Type type) => WriteExtendedList.ContainsKey(type.GetHashCode());

        /// <summary>
        /// Use <see cref="ExtendWriteContains"/> before calling this.
        /// </summary>
        protected static void ExtendWriteExecute<T>(PacketStream stream, T value) { WriteExtendedList[typeof (T).GetHashCode()](stream, value); }

        #endregion ExtendWrite

        public abstract void Write<T>(T value = default(T));


        public abstract void Dispose();
    }
}
