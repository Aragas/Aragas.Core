using System;
using System.Text;

using Newtonsoft.Json;

using Org.BouncyCastle.Crypto.Digests;

namespace Aragas.Core.Data
{
    public class PasswordStorage
    {
        private const string NoPassword = "PUT_PASSWORD_HERE";

        [JsonProperty("Hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Hash { get; private set; }

        [JsonProperty("Password", NullValueHandling = NullValueHandling.Ignore)]
        private string Password { get; set; }


        public PasswordStorage()
        {
            Password = NoPassword;
        }

        public PasswordStorage(string data, bool doHash = true)
        {
            if (doHash)
            {
                Password = data;

                var pswBytes = Encoding.UTF8.GetBytes(data);

                var sha512 = new Sha512Digest();
                var hashedPassword = new byte[sha512.GetDigestSize()];
                sha512.BlockUpdate(pswBytes, 0, pswBytes.Length);
                sha512.DoFinal(hashedPassword, 0);

                Hash = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();

                Password = null;
            }
            else
                Hash = data;
        }


        public PasswordStorage HashPassword()
        {
            if (!string.IsNullOrEmpty(Password) && Password != NoPassword)
            {
                var pswBytes = Encoding.UTF8.GetBytes(Password);

                var sha512 = new Sha512Digest();
                var hashedPassword = new byte[sha512.GetDigestSize()];
                sha512.BlockUpdate(pswBytes, 0, pswBytes.Length);
                sha512.DoFinal(hashedPassword, 0);

                Hash = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();

                Password = null;
            }
            else
                Password = NoPassword;
            
            return this;
        }
        
        public override string ToString()
        {
            return Hash;
        }
    }

    public class PasswordHandler : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is string)
                return new PasswordStorage((string) reader.Value);

            else if (reader.Value == null)
                return serializer.Deserialize<PasswordStorage>(reader).HashPassword(); // if Password is set, hash it.
            
            else
                return existingValue ?? new PasswordStorage();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is PasswordStorage)
                serializer.Serialize(writer, (PasswordStorage) value);
            
            else if (value is string)
                serializer.Serialize(writer, new PasswordStorage((string) value));

            else
                serializer.Serialize(writer, new PasswordStorage());
        }
    }
}
