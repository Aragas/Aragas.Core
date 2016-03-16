using System;
using System.Collections.Generic;

namespace Aragas.Core.IO
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PacketDataReader : IDisposable
    {
        public abstract bool IsServer { get; }


        #region ExtendRead

        private static readonly Dictionary<int, Func<PacketDataReader, int, object>> ReadExtendedList = new Dictionary<int, Func<PacketDataReader, int, object>>();

        public static void ExtendRead<T>(Func<PacketDataReader, int, T> func) { ReadExtendedList.Add(typeof(T).GetHashCode(), Change(func)); }

        private static Func<PacketDataReader, int, object> Change<T>(Func<PacketDataReader, int, T> action) => action == null ? (Func<PacketDataReader, int, object>) null : ((reader, length) => action(reader, length));

        protected static bool ExtendReadContains<T>() => ExtendReadContains(typeof(T));

        protected static bool ExtendReadContains(Type type) => ReadExtendedList.ContainsKey(type.GetHashCode());

        /// <summary>
        /// Use <see cref="ExtendReadContains"/> before calling this.
        /// </summary>
        protected static T ExtendReadExecute<T>(PacketDataReader reader, int length = 0) => (T) ReadExtendedList[typeof(T).GetHashCode()](reader, length);

        #endregion ExtendRead


        public abstract T Read<T>(T value = default(T), int length = 0);

        public abstract int BytesLeft();

        
        public abstract void Dispose();
    }
}
