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
        private readonly ITCPClient _tcp;

        private readonly BufferedBlockCipher _decryptCipher;
        private readonly BufferedBlockCipher _encryptCipher;

        public BouncyCastleAES(ITCPClient tcp, byte[] key)
        {
            _tcp = tcp;

            _encryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _encryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            _decryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _decryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }

        public void EncryptByteArray(byte[] array)
        {
            var encrypted = _encryptCipher.ProcessBytes(array, 0, array.Length);
            _tcp.WriteByteArray(encrypted);
        }
        public byte[] DecryptByteArray(int length)
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