using System;

namespace Aragas.Core.Interfaces
{
    /// <summary>
    /// Object that implements AES.
    /// </summary>
    public interface IAesStream : IDisposable
    {
        void EncryptByteArray(Byte[] array);
        Byte[] DecryptByteArray(Int32 length);
    }
}