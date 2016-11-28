using System.Security.Cryptography;
using System;
using System.IO;
using System.Text;

namespace cryptography.encryption
{
    public class SymmetricEncryption
    {
        public static byte[] GenerateKey(int keySize)
        {
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[keySize];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public static byte[] GenerateRandomNumber(int length)
        {
             using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[length];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public static EncryptedPacket Encrypt(string toBeEncrypted, byte[] key, byte[] initializationVector=null)
        {
            var valueInBytes = Encoding.UTF8.GetBytes(toBeEncrypted);
            return Encrypt(valueInBytes,key,initializationVector);
        }
        

        public static EncryptedPacket Encrypt(byte[] toBeEncrypted, byte[] key, byte[] initializationVector=null)
        {
            using(var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = initializationVector;

                using(var stream = new MemoryStream())
                using (var encryptionStream = new CryptoStream(stream,aes.CreateEncryptor(),CryptoStreamMode.Write))
                {
                    encryptionStream.Write(toBeEncrypted,0,toBeEncrypted.Length);
                    encryptionStream.FlushFinalBlock();

                    var encryptedData = stream.ToArray();

                    return new EncryptedPacket{
                        EncryptedData = encryptedData,
                        InitializationVector = initializationVector,
                        Method = "AES",
                    };
                   
                }
                
            }
        }

        public static string DecryptValue(byte[] toBeDecrypted, byte[] key, byte[] initializationVector)
        {
            var result = Decrypt(toBeDecrypted,key,initializationVector);
            var valueAsString = Encoding.UTF8.GetString(result.DecryptedData);

            return valueAsString;
        }

        public static DecryptedPacket Decrypt(byte[] toBeDecrypted, byte[] key, byte[] initializationVector)
        {
            using(var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = initializationVector;

                using(var stream = new MemoryStream())
                using (var encryptionStream = new CryptoStream(stream,aes.CreateDecryptor(),CryptoStreamMode.Write))
                {
                    encryptionStream.Write(toBeDecrypted,0,toBeDecrypted.Length);
                    encryptionStream.FlushFinalBlock();

                    var encryptedData = stream.ToArray();

                    return new DecryptedPacket{
                        DecryptedData = encryptedData,
                        InitializationVector = initializationVector,
                        Method = "AES",
                    };
                   
                }

            }
        }
    }
}