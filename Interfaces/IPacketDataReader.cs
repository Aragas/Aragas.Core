using System;

namespace Aragas.Core.Interfaces
{
    public interface IPacketDataReaderRead
    {
        T Read<T>(T value = default(T), Int32 length = 0);

        Int32 BytesLeft();
    }

    public interface IPacketDataReader : IPacketDataReaderRead, IDisposable
    {
        Boolean IsServer { get; }
    }
}
