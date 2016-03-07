using System;
using System.Text;

using Org.BouncyCastle.Crypto.Digests;

namespace Aragas.Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class PasswordStorage
    {
        private const string NoPassword = "PUT_PASSWORD_HERE";

        public string Hash
        {
            get
            {
                if (!string.IsNullOrEmpty(Password))
                    HashPassword();
                
                return _hash;
            }
            private set { _hash = value; }
        }
        private string _hash = string.Empty;

        public string Password { get; set; } = NoPassword;


        public PasswordStorage() { }

        public PasswordStorage(string data, bool doHash = true)
        {
            if (doHash)
            {
                Password = data;
                HashPassword();
            }
            else
            {
                Hash = data;
                Password = string.Empty;
            }
        }


        private void HashPassword()
        {
            if (string.IsNullOrEmpty(Password))
                Password = NoPassword;

            var pswBytes = Encoding.UTF8.GetBytes(Password);

            var sha512 = new Sha512Digest();
            var hashedPassword = new byte[sha512.GetDigestSize()];
            sha512.BlockUpdate(pswBytes, 0, pswBytes.Length);
            sha512.DoFinal(hashedPassword, 0);

            Hash = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();
            Password = string.Empty;
        }
        
        public override string ToString() => Hash;
    }
}
