using System;
using System.Collections.Generic;

using ExtendReadFunc = System.Func<Aragas.Core.IO.PacketDataReader, int, object>;

namespace Aragas.Core.IO
{
    public abstract class PacketDataReader : IDisposable
    {
        public abstract Boolean IsServer { get; }


        #region ExtendRead

        private static Dictionary<Int32, ExtendReadFunc> ReadExtendedList = new Dictionary<Int32, ExtendReadFunc>();

        public static void ExtendRead<T>(ExtendReadFunc func)
        {
            ReadExtendedList.Add(typeof(T).GetHashCode(), func);
        }

        protected static bool ExtendReadContains<T>()
        {
            return ExtendReadContains(typeof(T));
        }
        protected static bool ExtendReadContains(Type type)
        {
            return ReadExtendedList.ContainsKey(type.GetHashCode());
        }

        /// <summary>
        /// Use <see cref="ExtendReadContains"/> before calling this.
        /// </summary>
        protected static T ExtendReadExecute<T>(PacketDataReader reader, Int32 length = 0)
        {
            return (T) ReadExtendedList[typeof(T).GetHashCode()](reader, length);
        }

        #endregion ExtendRead


        public abstract T Read<T>(T value = default(T), Int32 length = 0);

        public abstract Int32 BytesLeft();

        
        public abstract void Dispose();
    }
}
