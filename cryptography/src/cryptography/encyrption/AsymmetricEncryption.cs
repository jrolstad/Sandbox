using System.Security.Cryptography;

namespace cryptography.encryption
{
    public class AsymmetricEncryption
    {
        public static RSAKeyPair GenerateKey(int keySize)
        {
            using(var rsa = RSA.Create())
            {
                var publicKey = rsa.ExportParameters(false);
                var privateKey = rsa.ExportParameters(true);

                return new RSAKeyPair{
                    PublicKey = publicKey,
                    PrivateKey = privateKey
                };
            }
        }

        public static EncryptedPacket Encrypt(string toBeEncrypted, RSAParameters publicKey)
        {
            var dataAsBytes = System.Text.Encoding.UTF8.GetBytes(toBeEncrypted);
            return Encrypt(dataAsBytes,publicKey);
        }

        public static EncryptedPacket Encrypt(byte[] toBeEncrypted, RSAParameters publicKey)
        {
            using(var rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                var encryptedData = rsa.Encrypt(toBeEncrypted,RSAEncryptionPadding.OaepSHA1);

                return new EncryptedPacket{
                    EncryptedData = encryptedData,
                    Method = "RSA"
                };
            }
        }

        public static string DecryptValue(byte[] toBeDecrypted, RSAParameters privateKey)
        {
            var decryptionResult = Decrypt(toBeDecrypted,privateKey);
            var value = System.Text.Encoding.UTF8.GetString(decryptionResult.DecryptedData);

            return value;
        }
        
        public static DecryptedPacket Decrypt(byte[] toBeDecrypted, RSAParameters privateKey)
        {
            using(var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                var encryptedData = rsa.Decrypt(toBeDecrypted,RSAEncryptionPadding.OaepSHA1);

                return new DecryptedPacket{
                    DecryptedData = encryptedData,
                    Method = "RSA"
                };
            }
        }
    }
}