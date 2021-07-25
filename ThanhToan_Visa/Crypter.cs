using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Configuration;
using System.Text;

namespace LAPTRINHWEB.ThanhToan_Visa
{
    public static class Crypter
    {

        static RsaKeyParameters rsaKeyParameters;
        static Crypter()
        {
            var keyInfoData = Convert.FromBase64String(Config.publicKey);
            rsaKeyParameters = PublicKeyFactory.CreateKey(keyInfoData) as RsaKeyParameters;
        }

        public static string Encrypt(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new TickDateTimeConverter() }
            };

            var serialized = JsonConvert.SerializeObject(obj, settings);
            var payloadBytes = Encoding.UTF8.GetBytes(serialized);

            var cipher = GetAsymmetricBlockCipher(true);
            var encrypted = Process(cipher, payloadBytes);

            var encoded = Convert.ToBase64String(encrypted);
            return encoded;
        }

        public static T Decrypt<T>(string encryptedText)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new TickDateTimeConverter() }
            };

            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

            var cipher = GetAsymmetricBlockCipher(false);
            var decrypted = Process(cipher, cipherTextBytes);

            var decoded = Encoding.UTF8.GetString(decrypted);

            return JsonConvert.DeserializeObject<T>(decoded, settings);
        }

        private static IAsymmetricBlockCipher GetAsymmetricBlockCipher(bool forEncryption)
        {
            var cipher = new Pkcs1Encoding(new RsaEngine());
            cipher.Init(forEncryption, rsaKeyParameters);

            return cipher;
        }

        private static byte[] Process(IAsymmetricBlockCipher cipher, byte[] payloadBytes)
        {
            int length = payloadBytes.Length;
            int blockSize = cipher.GetInputBlockSize();

            var plainTextBytes = new List<byte>();
            for (int chunkPosition = 0; chunkPosition < length; chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                plainTextBytes.AddRange(cipher.ProcessBlock(
                    payloadBytes, chunkPosition, chunkSize
                ));
            }

            return plainTextBytes.ToArray();
        }
    }
}