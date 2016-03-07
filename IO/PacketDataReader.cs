using System;
using System.Collections.Generic;

using ExtendReadFunc = System.Func<Aragas.Core.IO.PacketDataReader, int, object>;

namespace Aragas.Core.IO
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PacketDataReader : IDisposable
    {
        public abstract bool IsServer { get; }


        #region ExtendRead

        private static readonly Dictionary<int, ExtendReadFunc> ReadExtendedList = new Dictionary<int, ExtendReadFunc>();

        public static void ExtendRead<T>(ExtendReadFunc func) { ReadExtendedList.Add(typeof(T).GetHashCode(), func); }

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
