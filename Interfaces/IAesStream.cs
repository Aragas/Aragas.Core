using System;

namespace Aragas.Core.Interfaces
{
    /// <summary>
    /// Object that implements AES.
    /// </summary>
    public interface IAesStream : IDisposable
    {
        Int32 Read(Byte[] buffer, Int32 offset, Int32 count);

        void Write(Byte[] buffer, Int32 offset, Int32 count);
    }
}