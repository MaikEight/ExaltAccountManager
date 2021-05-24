using System.IO;
using System.Security.Cryptography;

namespace ExaltAccountManager
{
    public class AesCryptographyService
    {
        byte[] key = new byte[16] { 0xB0, 0x10, 0x00, 0x00, 0x0AC, 0x06, 0x20, 0x03, 0xF3, 0xFF, 0x13, 0x00, 0x59, 0x99, 0x00, 0x80 };
        byte[] iv = new byte[16] { 0x00, 0x20, 0xDE, 0x9F, 0x8, 0x00, 0x08, 0x00, 0x11, 0x00, 0x00, 0x0B, 0x00, 0x2F, 0xA0, 0x01 };
        //Yes, I know that everybody got the same key.. but this should be good enough for a tool like this.

        public byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                //aes.Padding = PaddingMode.Zeros;

                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, encryptor);
                }
            }
        }

        public byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;

                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, decryptor);
                }
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        public byte[] Encrypt(byte[] data)
        {
            var crypto = new AesCryptographyService();

            return crypto.Encrypt(data, key, iv);
        }

        public byte[] Decrypt(byte[] encData)
        {
            var crypto = new AesCryptographyService();

            return crypto.Decrypt(encData, key, iv);
        }
    }
}
