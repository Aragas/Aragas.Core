using System;
using System.Collections.Generic;

namespace Aragas.Core.Interfaces
{
    public abstract class PacketDataReader : IDisposable
    {
        public abstract Boolean IsServer { get; }


        #region Read

        public class ExtendReadInfo
        {
            public Type Type { get; }
            public Func<PacketDataReader, int, object> Function { get; }

            public ExtendReadInfo(Type type, Func<PacketDataReader, int, object> func)
            {
                Type = type;
                Function = func;
            }
        }

        protected static List<ExtendReadInfo> ReadExtendedList = new List<ExtendReadInfo>();
        public static void ExtendRead(ExtendReadInfo ext) { ReadExtendedList.Add(ext); }
        public abstract T Read<T>(T value = default(T), Int32 length = 0);

        public abstract Int32 BytesLeft();

        #endregion Read


        public abstract void Dispose();
    }
}
