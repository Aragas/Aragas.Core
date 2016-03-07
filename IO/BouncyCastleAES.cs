using System.IO;

using Aragas.Core.Wrappers;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Aragas.Core.IO
{
    /// <summary>
    /// BouncyCastle's AesStream implementation.
    /// </summary>
    public partial class BouncyCastleAes : AesStream
    {
        private ITCPClient TCPClient { get; }
        protected override Stream BaseStream => TCPClient.GetStream();

        private BufferedBlockCipher DecryptCipher { get; }
        private BufferedBlockCipher EncryptCipher { get; }

        public BouncyCastleAes(ITCPClient tcp, byte[] key)
        {
            TCPClient = tcp;

            EncryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            EncryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            DecryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            DecryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }
    }
}