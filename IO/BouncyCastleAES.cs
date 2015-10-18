using System;

using Aragas.Core.Interfaces;
using Aragas.Core.Wrappers;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Aragas.Core.IO
{
    public sealed class BouncyCastleAES : IAesStream
    {
        private readonly INetworkTCPClient _tcp;

        private readonly BufferedBlockCipher _decryptCipher;
        private readonly BufferedBlockCipher _encryptCipher;

        public BouncyCastleAES(INetworkTCPClient tcp, byte[] key)
        {
            _tcp = tcp;

            _encryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _encryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            _decryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _decryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            var encrypted = _encryptCipher.ProcessBytes(buffer, offset, count);
            _tcp.Send(encrypted, 0, encrypted.Length);
        }
        public int Read(byte[] buffer, int offset, int count)
        {
            var length = _tcp.Receive(buffer, offset, count);
            var decrypted = _decryptCipher.ProcessBytes(buffer, offset, length);
            Buffer.BlockCopy(decrypted, 0, buffer, offset, decrypted.Length);
            return length;
        }
        public byte[] ReadByteArray(int length)
        {
            var buffer = _tcp.ReadByteArray(length);
            return _decryptCipher.ProcessBytes(buffer, 0, length);
        }

        public void Dispose()
        {
            _decryptCipher?.Reset();

            _encryptCipher?.Reset();
        }
    }
}